using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FMOD;
using Debug = UnityEngine.Debug;

public class NoteManager : MonoBehaviour
{
    public float earlywindow = .6f;
    public float latewindow = .6f;
    public float notedelay = 0;
    public int correctnotes;
    public int incorrectnotes;

    public List<Note> activenotes = new List<Note>();
    public List<Note> pressednotes = new List<Note>();
    public int numberofactiveleaders;
    public ListeningBoard listeningBoard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        // 1) First, remove any notes that have expired
        for (int idx = activenotes.Count - 1; idx >= 0; idx--)
        {
            var note = activenotes[idx];
            if (note.endtime < Time.time - latewindow)
            {
                Debug.Log($"Deactivating note: {note.noteID}, {note.endtime}");
                note.incorrect();
                activenotes.RemoveAt(idx);
                incorrectnotes++;
            }
        }

        // 2) Now, among the remaining future notes, pick out the next
        //    `numberofactiveleaders` chords (by starttime), and activate all notes in them.
        var futureNotes = activenotes
            .Where(n => n.starttime > Time.time)
            .OrderBy(n => n.starttime)
            .ToList();

        float lastStartTime = -Mathf.Infinity;
        int chordCount    = 0;

        foreach (var note in futureNotes)
        {
            // if this note’s starttime is “far enough” from the last chord, it’s a new chord
            if (Mathf.Abs(note.starttime - lastStartTime) > 0.1f)
            {
                chordCount++;
                lastStartTime = note.starttime;

                // if we’ve already done N chords, stop entirely
                if (chordCount > numberofactiveleaders)
                    break;
            }

            // as long as chordCount ≤ numberofactiveleaders, activate every note in that chord
            note.activateleader();
        }
    }

    public bool checkKeyHit(int keyID){
        foreach (Note i in activenotes){
            if (i.starttime > Time.time - earlywindow && i.starttime < Time.time + latewindow && i.noteID == keyID){
                UnityEngine.Debug.Log("Note hit: HIT: " + Time.time + " : " + i.starttime);
                i.activate();
                pressednotes.Add(i);
                activenotes.Remove(i);
                Vector3 newpos = i.Header.transform.position;
                float judgez = listeningBoard.judgementLine.transform.position.z;
                float prevz = newpos.z;
                Debug.Log(judgez + " : " + prevz);
                newpos.z = listeningBoard.judgementLine.transform.position.z;
                //newpos.z = 12;
                
                i.Header.transform.position =newpos;
                return true;
            }
        }
        Debug.Log("Note hit: MISS "+ Time.time);
        return false;
    }
    public bool checkKeyRelease(int keyID)
    {
        foreach (Note i in pressednotes)
        {
            if (i.noteID == keyID)
            {
                Debug.Log("Note Release: Note Found " + i);
                if (i.endtime > Time.time - earlywindow
                && i.endtime < Time.time + latewindow)
                {
                    Debug.Log("Note Release: HIT " + Time.time + " : " + i.endtime);
                    i.correct();
                    pressednotes.Remove(i);
                    correctnotes++;
                    return true;
                }
                Debug.Log("Note Release: MISS " + Time.time +" : " + i.endtime);
                i.incorrect();
                pressednotes.Remove(i);
                incorrectnotes++;
                return false;
            }
        }
        Debug.Log("Note Release: Note Not Found ");
        return false;
    }
}


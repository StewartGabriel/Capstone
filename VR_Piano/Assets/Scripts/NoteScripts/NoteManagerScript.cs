using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NoteManager : MonoBehaviour
{
    public float notebuffer = .1f;
    public float notedelay = 0;
    public int correctnotes;
    public int incorrectnotes;

    public List<Note> activenotes = new List<Note>();
    public List<Note> pressednotes = new List<Note>();
    public int numberofactiveleaders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        int chordCount = 0;
        float lastStartTime = 0;

        // Loop through the active notes and handle deactivation and leader activation
        foreach (Note i in activenotes)
        {
            // Deactivate notes based on endtime
            if (i.endtime < Time.time - notebuffer)
            {
                Debug.Log("Deactivating note: " + i.noteID + ", " + i.endtime);
                i.deactivate();
                activenotes.Remove(i);
                incorrectnotes++;
                continue; // Move to the next note if this one is deactivated
            }

            // Check if this note is in the future and is part of a chord
            if (i.starttime > Time.time && chordCount < numberofactiveleaders)
            {
                if (Mathf.Abs(i.starttime - lastStartTime) > 0.1f)
                {
                    chordCount++;
                    lastStartTime = i.starttime;
                }
                i.activateleader();  // Activate leader only for notes that are part of the chord
            }
        }
    }
    public bool checkKeyHit(int keyID){
        foreach (Note i in activenotes){
            if (i.starttime > Time.time - notebuffer && i.starttime < Time.time + notebuffer && i.noteID == keyID){
                Debug.Log("Note hit: HIT: " + Time.time + " : " + i.starttime);
                i.activate();
                pressednotes.Add(i);
                activenotes.Remove(i);
                return true;
            }
        }
        Debug.Log("Note hit: MISS "+ Time.time);
        return false;
    }
    public bool checkKeyRelease(int keyID){
        foreach (Note i in pressednotes){
            if (i.endtime > Time.time - notebuffer 
             && i.endtime < Time.time + notebuffer 
             && i.noteID == keyID){
                Debug.Log("Note Release: HIT " + Time.time + " : " + i.starttime);
                i.correct();
                pressednotes.Remove(i);
                correctnotes++;
                return true;
            }
        }
        Debug.Log("Note Release: MISS "+ Time.time);
        return false;
    }
}


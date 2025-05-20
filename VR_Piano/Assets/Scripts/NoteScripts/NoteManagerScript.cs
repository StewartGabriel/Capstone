using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NoteManager : MonoBehaviour
{
    public float notebuffer = .1f;
    public float notedelay = 0;
    public List<Note> activenotes  = new List<Note>();
    public List<Note> pressednotes = new List<Note>();
    public int numberofactiveleaders;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Note i in activenotes)
        {
            if (i.endtime < Time.time -notebuffer)
            {
                Debug.Log("Deactivating note: " + i.noteID + ", " + i.endtime);
                i.deactivate();
            }
        }

        // Get the 3 notes closest to start time (future ones only)
        List<Note> closestToEnd = activenotes
            .Where(note => note.starttime > Time.time)
            .OrderBy(note => note.starttime)
            .ToList();

        int chordCount = 0;
        float lastStartTime = 0;

        foreach (Note note in closestToEnd)
        {
            float currentStartTime = note.starttime;

            if (chordCount >= numberofactiveleaders) break;

            // Only increment chordCount if this note is not part of the previous chord
            if (Mathf.Abs(currentStartTime - lastStartTime) > 0.1f)
            {
                chordCount++;
                lastStartTime = currentStartTime;
            }

            note.activateleader();
        }
    }
}

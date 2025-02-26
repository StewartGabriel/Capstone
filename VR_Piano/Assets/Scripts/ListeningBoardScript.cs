using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ListeningBoard : NoteCallback
{
    public float notedelay;
    private readonly Key[] keys = { Key.S, Key.D, Key.F };
    public TalkingBoard talkingboard;
    public List<Note> activenotes = new List<Note>();
    void Start()
    {
        Vector3 talkingboardspawnposition = this.transform.position;
        talkingboardspawnposition.z  = talkingboardspawnposition.z + notedelay;  
        TalkingBoard playetalkingboard = Instantiate(talkingboard,talkingboardspawnposition, Quaternion.identity);
        CreateBoard();
    }

     void Update()
    {
       if (Keyboard.current == null) return; // Ensure keyboard input exists

        for (int i = 0; i < keys.Length; i++)
        {
            var keyControl = Keyboard.current[keys[i]]; // Get key reference

            if (keyControl != null)
            {
                if (keyControl.wasPressedThisFrame)
                {
                    KeySet[i].KeyDown(Random.Range(0, 128));
                }
                if (keyControl.wasReleasedThisFrame)
                {
                    KeySet[i].KeyUp();
                }
            }
        }
    }

    private void CheckForNoteHit()
    {
        if (activenotes.Count > 0)
        {
            Note hitNote = activenotes[0];
            activenotes.Remove(hitNote);
            Destroy(hitNote);
            Debug.Log("Note hit!");
        }
        else
        {
            Debug.Log("Miss!");
        }
    }
     private void OnTriggerEnter(Collider other)
    {
         Note note = other.GetComponent<Note>();
        if (note != null)
        {
            activenotes.Add(note);
            note.activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            activenotes.Remove(note);
            note.deactivate();
        }
    }
}
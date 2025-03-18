using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ListeningBoard : NoteCallback
{
    public float notedelay;
    //private Key[] keys = { Key.S, Key.D, Key.F };
    public TalkingBoard talkingboard;
    //public NoteManager noteManager;
    void Start()
    {
        notemanager.notedelay = notedelay;
        Vector3 talkingboardspawnposition = transform.position;
        talkingboardspawnposition.z += notedelay;
        Instantiate(talkingboard, talkingboardspawnposition, Quaternion.identity);
        talkingboard.notemanager = notemanager;
        base.Start();
    }

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame) KeySet[0].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.sKey.wasReleasedThisFrame) KeySet[0].KeyUp();
        if (Keyboard.current.dKey.wasPressedThisFrame) KeySet[1].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.dKey.wasReleasedThisFrame) KeySet[1].KeyUp();
        if (Keyboard.current.fKey.wasPressedThisFrame) KeySet[2].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.fKey.wasReleasedThisFrame)KeySet[2].KeyUp();    }

    
    // private void OnTriggerEnter(Collider other)
    // {
    //     Note note = other.GetComponent<Note>();
    //     if (note != null)
    //     {
    //         talkingboard.activenotes.Add(note);
    //         note.activate();
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        // Note note = other.GetComponent<Note>();
        // if (note != null)
        // {
        //     talkingboard.activenotes.Remove(note);
        //     note.deactivate();
        // }
    }
}

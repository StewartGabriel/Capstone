using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ListeningBoard : PianoKeyboard
{
    public float notedelay;
    public TalkingBoard talkingboard;
    void Awake()
    {
        notemanager.notedelay = notedelay;
        Vector3 talkingboardspawnposition = transform.position;
        talkingboardspawnposition.z += notedelay;
        talkingboard.FirstNoteID = FirstNoteID;
        talkingboard.KeyCount = KeyCount;
        talkingboard.notemanager = notemanager;
        talkingboard = Instantiate(talkingboard, talkingboardspawnposition, Quaternion.identity);
        talkingboard.transform.parent = this.transform.parent;
        base.Awake();
    }

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame) KeySet[0].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.sKey.wasReleasedThisFrame) KeySet[0].KeyUp();
        if (Keyboard.current.dKey.wasPressedThisFrame) KeySet[1].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.dKey.wasReleasedThisFrame) KeySet[1].KeyUp();
        if (Keyboard.current.fKey.wasPressedThisFrame) KeySet[2].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.fKey.wasReleasedThisFrame)KeySet[2].KeyUp();    
    }

        public void InterpretMidi(int note, int velocity)
    {
        int t = note - 1 - FirstNoteID;
        Debug.Log("Note Received From Library: " + note + ", " + t + "Array Size:" + KeySet.Length);

        if (velocity > 0)
        {
            KeySet[note - 1 - FirstNoteID].KeyDown(velocity);
        }
        else
        {
            KeySet[note - 1 - FirstNoteID].KeyUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class ListeningBoard : PianoKeyboard
{
    public float notedelay;
    public TalkingBoard talkingboard;

    private EventInstance pianoEvent; // For MIDI
    private string parameterName = "note"; // For laptop keyboard testing
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

        pianoEvent = RuntimeManager.CreateInstance("event:/Piano Sounds");

        if (!pianoEvent.isValid())
        {
            Debug.LogError("FMOD Event: Piano Sounds, not found.");
        }

        base.Awake();
    }

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            KeySet[0].KeyDown(Random.Range(0, 128));
            pianoEvent.setParameterByName(parameterName, 0f);
            pianoEvent.start();
        }

        if (Keyboard.current.sKey.wasReleasedThisFrame) KeySet[0].KeyUp();

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            KeySet[1].KeyDown(Random.Range(0, 128));
            pianoEvent.setParameterByName(parameterName, 1f);
            pianoEvent.start();
        }

        if (Keyboard.current.dKey.wasReleasedThisFrame) KeySet[1].KeyUp();

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            KeySet[2].KeyDown(Random.Range(0, 128));
            pianoEvent.setParameterByName(parameterName, 2f);
            pianoEvent.start();
        }

        if (Keyboard.current.fKey.wasReleasedThisFrame)KeySet[2].KeyUp();    
    }

        public void InterpretMidi(int note, int velocity)
    {
        int t = note - 1 - FirstNoteID;
        Debug.Log("Note Received: " + note + ", " + t + "Array Size:" + KeySet.Length);

        pianoEvent.setParameterByName("note", note);


        if (velocity > 0)
        {
            KeySet[note - 1 - FirstNoteID].KeyDown(velocity);
            pianoEvent.start();
        }
        else
        {
            KeySet[note - 1 - FirstNoteID].KeyUp();

        }
    }
}

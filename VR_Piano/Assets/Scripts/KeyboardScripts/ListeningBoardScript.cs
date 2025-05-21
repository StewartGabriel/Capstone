using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

using Random = UnityEngine.Random;


public class ListeningBoard : PianoKeyboard
{
    public PianoHandle lefthandle;
    public PianoHandle righthandle;
    public NoteBoard noteboard;
    public float notedelay;
    public TalkingBoard talkingboard;



    private string parameterName = "note";

    void Awake()
    {
        lefthandle.sethandleposition();
        righthandle.sethandleposition();
        
        // Retrieve values from PlayerPrefs with default values to prevent issues if the keys don't exist
        int leftmostNote = PlayerPrefs.GetInt("PianoHandle1_LeftmostNote", 28);
        int rightmostNote = PlayerPrefs.GetInt("PianoHandle1_RightmostNote", 116);

        // Calculate KeyCount and FirstNoteID
        KeyCount = rightmostNote - leftmostNote;
        FirstNoteID = leftmostNote;

        notemanager.notedelay = notedelay;
        talkingboard.FirstNoteID = FirstNoteID;
        talkingboard.KeyCount = KeyCount;
        talkingboard.notemanager = notemanager;

        float PianoHandledimensions = KeyPreFab.transform.localScale.z / 2;
        lefthandle.transform.localScale = new Vector3(PianoHandledimensions, PianoHandledimensions, PianoHandledimensions);
        righthandle.transform.localScale = new Vector3(PianoHandledimensions, PianoHandledimensions, PianoHandledimensions);

        //pianoEvent = RuntimeManager.CreateInstance("event:/Piano Sounds");

        //if (!pianoEvent.isValid())
        //{
        //    Debug.LogError("FMOD Event: Piano Sounds, not found.");
        //}

        base.Awake();

        talkingboard = Instantiate(talkingboard);
        talkingboard.transform.position = transform.position + new Vector3(0, 0, notedelay); //Not sure why the -1 is needed but it properly places the talking board
        talkingboard.transform.rotation = Quaternion.identity;
        talkingboard.transform.SetParent(transform, worldPositionStays: true); // retains correct world scale

        Vector3 noteboardspawnposition = transform.position + new Vector3(0, 0, notedelay / 2);
        //noteboardspawnposition.z = (transform.position.z + talkingboard.transform.position.z);
        noteboardspawnposition.y = transform.position.y + transform.lossyScale.y / 2 - noteboard.transform.lossyScale.y;

        noteboard = Instantiate(noteboard, noteboardspawnposition, quaternion.identity);
        noteboard.BuildBoard(transform.lossyScale.x, notedelay);
        noteboard.transform.parent = this.transform;
        noteboard.BuildFrets(fretlocations, spacing);
        
    }


    void Update()
    {
        // Test key inputs
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            KeySet[0].KeyDown(Random.Range(0, 128), true);
            StartPianoEvent(0f);
        }

        if (Keyboard.current.sKey.wasReleasedThisFrame)
        {
            KeySet[0].KeyUp();
            StopPianoEvent();
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            KeySet[1].KeyDown(Random.Range(0, 128), true);
            StartPianoEvent(1f);
        }

        if (Keyboard.current.dKey.wasReleasedThisFrame)
        {
            KeySet[1].KeyUp();
            StopPianoEvent();
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            KeySet[2].KeyDown(Random.Range(0, 128), false);
            StartPianoEvent(2f);
        }

        if (Keyboard.current.fKey.wasReleasedThisFrame)
        {
            KeySet[2].KeyUp();
            StopPianoEvent();
        }

        // Position the board between the handles
        Vector3 mid = (lefthandle.transform.position + righthandle.transform.position)/2;
        transform.position = mid;

        // Resize to span the width between handles in X-Z plane
        Vector3 flatLeft = new Vector3(lefthandle.transform.position.x, 0, lefthandle.transform.position.z);
        Vector3 flatRight = new Vector3(righthandle.transform.position.x, 0, righthandle.transform.position.z);
        float width = Vector3.Distance(flatLeft, flatRight);

        Vector3 dim = transform.localScale;
        dim.x = width;
        transform.localScale = dim;

        // Rotate board to face from left to right handle
        Vector3 direction = flatRight - flatLeft;
        if (direction != Vector3.zero)
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);

    }

    public void InterpretMidi(int note, int velocity, bool hand)
    {
        int index = note - FirstNoteID;
        Debug.Log($"Note Received From Library: {note}, {index} Array Size: {KeySet.Length}");

        if (index >= 0 && index < KeySet.Length)
        {
            if (velocity > 0)
            {
                KeySet[index].KeyDown(velocity, hand);
                StartPianoEvent(note);
            }

            else
            {
                KeySet[index].KeyUp();
                StopPianoEvent();
            }

        }
        else
        {
            Debug.LogWarning($"Note {note} is out of range for KeySet.");
        }
    }

    private void StartPianoEvent(float note)
    {
        EventInstance pianoEvent = RuntimeManager.CreateInstance("event:/Piano Sounds");
        pianoEvent.setParameterByName("note", note);
        pianoEvent.start();
        pianoEvent.release();
    }

    private void StopPianoEvent()
    {
        EventInstance pianoEvent = RuntimeManager.CreateInstance("event:/Piano Sounds");
        pianoEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        pianoEvent.release();
    }
}

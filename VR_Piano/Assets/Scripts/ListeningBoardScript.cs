using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ListeningBoard : PianoKeyboard
{
    public PianoHandle lefthandle;
    public PianoHandle righthandle;
    public float notedelay;
    public TalkingBoard talkingboard;

   void Awake()
    {
        notemanager.notedelay = notedelay;
        talkingboard.FirstNoteID = FirstNoteID;
        talkingboard.KeyCount = KeyCount;
        talkingboard.notemanager = notemanager;
        float PianoHandledimensions = KeyPreFab.transform.localScale.z/2;
        lefthandle.transform.localScale = new Vector3(PianoHandledimensions,PianoHandledimensions,PianoHandledimensions);
        righthandle.transform.localScale = new Vector3(PianoHandledimensions,PianoHandledimensions,PianoHandledimensions);

        base.Awake();

        talkingboard = Instantiate(talkingboard);
        talkingboard.transform.position = transform.position + new Vector3(0, 0, notedelay);
        talkingboard.transform.rotation = Quaternion.identity;
        //talkingboard.transform.localScale = Vector3.one; // or whatever scale you want
        talkingboard.transform.SetParent(transform, worldPositionStays: true); // retains correct world scale


    }


    void Update()
    {
        // Test key inputs
        if (Keyboard.current.sKey.wasPressedThisFrame) KeySet[0].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.sKey.wasReleasedThisFrame) KeySet[0].KeyUp();
        if (Keyboard.current.dKey.wasPressedThisFrame) KeySet[1].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.dKey.wasReleasedThisFrame) KeySet[1].KeyUp();
        if (Keyboard.current.fKey.wasPressedThisFrame) KeySet[2].KeyDown(Random.Range(0, 128));
        if (Keyboard.current.fKey.wasReleasedThisFrame) KeySet[2].KeyUp();

        // Position the board between the handles
        Vector3 mid = (lefthandle.transform.position + righthandle.transform.position) / 2;
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

    public void InterpretMidi(int note, int velocity)
    {
        int index = note - 1 - FirstNoteID;
        Debug.Log($"Note Received From Library: {note}, {index} Array Size: {KeySet.Length}");

        if (index >= 0 && index < KeySet.Length)
        {
            if (velocity > 0)
                KeySet[index].KeyDown(velocity);
            else
                KeySet[index].KeyUp();
        }
        else
        {
            Debug.LogWarning($"Note {note} is out of range for KeySet.");
        }
    }
}

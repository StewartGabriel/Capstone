using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class TalkingBoard : PianoKeyboard
{    
    void Awake()
    {
        base.Awake();
        UnityEngine.Vector3 targetscale = new UnityEngine.Vector3(
            transform.lossyScale.x/transform.parent.lossyScale.x,
            transform.localScale.y/transform.parent.lossyScale.y,
            transform.localScale.z/transform.parent.lossyScale.z);
        transform.localScale = targetscale;
    }
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            KeySet[0].KeyDown(Random.Range(0, 128), true);
            StartPianoEvent(FirstNoteID);
        }

        if (Keyboard.current.wKey.wasReleasedThisFrame)
        {
            KeySet[0].KeyUp();
            StopPianoEvent();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            KeySet[1].KeyDown(Random.Range(0, 128), false);
            StartPianoEvent(FirstNoteID);
        }

        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            KeySet[1].KeyUp();
            StopPianoEvent();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            KeySet[2].KeyDown(Random.Range(0, 128), true);
            StartPianoEvent(FirstNoteID);
        }
        if (Keyboard.current.rKey.wasReleasedThisFrame)
        {
            KeySet[2].KeyUp();
            StopPianoEvent();
        }
    }
    public void InterpretMidi(int note, int velocity, bool hand)
    {
        int t = note - 1 - FirstNoteID;
        Debug.Log("Note Received From Library: " + note + ", " + t + " Array Size:" + KeySet.Length + " FirstNoteID: " + FirstNoteID);
        


        if (velocity > 0)
        {
            try{
                KeySet[t].KeyDown(velocity, hand);
                StartPianoEvent(t);

            }
            catch (System.Exception e)
            {
                Debug.LogError("KeyDown error: " + e.Message + ", " + note + ", " + t + " Array Size:" + KeySet.Length);
                Debug.LogError(e.StackTrace);
            }
        }
        else
        {
            KeySet[t].KeyUp();
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

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

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
        if (Keyboard.current.wKey.wasPressedThisFrame) KeySet[0].KeyDown(Random.Range(0, 128), true);
        if (Keyboard.current.wKey.wasReleasedThisFrame) KeySet[0].KeyUp();
        if (Keyboard.current.eKey.wasPressedThisFrame) KeySet[1].KeyDown(Random.Range(0, 128), false);
        if (Keyboard.current.eKey.wasReleasedThisFrame) KeySet[1].KeyUp();
        if (Keyboard.current.rKey.wasPressedThisFrame) KeySet[2].KeyDown(Random.Range(0, 128), true);
        if (Keyboard.current.rKey.wasReleasedThisFrame)KeySet[2].KeyUp();
    }
    public void InterpretMidi(int note, int velocity, bool hand)
    {
        int t = note - 1 - FirstNoteID;
        Debug.Log("Note Received From Library: " + note + ", " + t + "Array Size:" + KeySet.Length);
        
        if (velocity > 0)
        {
            KeySet[note - 1 - FirstNoteID].KeyDown(velocity, hand);
        }
        else
        {
            KeySet[note - 1 - FirstNoteID].KeyUp();
        }
    }
    
}

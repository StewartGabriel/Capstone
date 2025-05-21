using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ListeningKeyScript : Key
{
    public TextMeshProUGUI labelText;
    string GetPianoKeyName(int midiNote)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        int octave = (midiNote / 12) - 1;
        string note = noteNames[midiNote % 12];
        //Debug.Log(midiNote + " : " + note);
        return note + octave;
    }
    void Start()
    {
        labelText.text = GetPianoKeyName(keyID);
        //base.Start();
    }
    public override void KeyDown(int speed, bool hand)
    {
        //Debug.Log("KeyDown called: " + keyID);
        base.KeyDown(speed, hand);
        if (noteManager.checkKeyHit(keyID))
        {
            thisKeysRenderer.material = Materials[3];
        }
        else
        {
            thisKeysRenderer.material = Materials[2];
        }

        // SoundManager.PlaySound(pianoSound, keyID,speed / 127f); // old sound func

    }
    public override void KeyUp()
    {
        //Debug.Log("KeyUp called: " + keyID);

        if(black)
            thisKeysRenderer.material = Materials[1];
        else
            thisKeysRenderer.material = Materials[0]; 
        
        transform.Translate(Vector3.up * transform.lossyScale.y, Space.Self);
        if (noteManager.checkKeyRelease(keyID))
        {
            thisKeysRenderer.material = Materials[2];
        }
    }
    
}

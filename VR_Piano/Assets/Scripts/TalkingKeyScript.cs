using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingKeyScript : Key
{
    public Note NotePrefab; 
    public override void KeyDown(int speed)
    {
        base.KeyDown(speed);
        Vector3 spawnPosition = transform.position + transform.forward * (transform.lossyScale.z / 2);

//        Debug.Log("Spawning new note");
        Note newNote = Instantiate(
                NotePrefab,spawnPosition,
                Quaternion.identity
            );    
        newNote.transform.SetParent(this.transform);
        newNote.noteID = keyID;
        newNote.starttime = Time.time + noteManager.notedelay;
        newNote.endtime = float.MaxValue;
        currentnote = newNote;
        if (black){newNote.notematerials[0] = newNote.notematerials[4];}
        noteManager.activenotes.Add(currentnote);
        
        SoundManager.PlaySound(pianoSound, keyID ,speed / 127f);
    }
    public override void KeyUp()
    {
        //Debug.Log("KeyUp called: " + keyID);
        thisKeysRenderer.material = Materials[0];
        
        if (currentnote != null)
        {
            currentnote.endtime = Time.time + noteManager.notedelay;
            currentnote.on = false;
            currentnote = null;
        }
        transform.Translate(Vector3.up * .05f, Space.Self);
    }
}

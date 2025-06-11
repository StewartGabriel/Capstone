using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingKeyScript : Key
{
    public Note NotePrefab; 
    public override void KeyDown(int speed, bool hand)
    {
        base.KeyDown(speed, hand);
        Vector3 spawnPosition = transform.position;
        spawnPosition.z = transform.parent.position.z;

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
        if (black && hand){newNote.notematerials[0] = newNote.notematerials[4];}
        if (!black && !hand ){newNote.notematerials[0] = newNote.notematerials[5];}
        if (black && !hand){newNote.notematerials[0] = newNote.notematerials[6];}
        
        noteManager.activenotes.Add(currentnote);
        
        //SoundManager.PlaySound(pianoSound, keyID ,speed / 127f); <---- This side shouldn't be playing sounds, so I commented it out. 
    }
    public override void KeyUp()
    {
        base.KeyUp();
        if (currentnote != null)
        {
            currentnote.endtime = Time.time + noteManager.notedelay;
            currentnote.on = false;
            currentnote = null;
        }
        
    }
}

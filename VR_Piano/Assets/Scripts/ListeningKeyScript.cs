using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class ListeningKeyScript : Key
{
     public override void KeyDown(int speed)
    {
        //Debug.Log("KeyDown called: " + keyID);
        base.KeyDown(speed);
        if(checkKeyHit()){
            thisKeysRenderer.material = Materials[2];
        }

        SoundManager.PlaySound(pianoSound, speed / 127f);

    }
    public override void KeyUp()
    {
        //Debug.Log("KeyUp called: " + keyID);
        
        thisKeysRenderer.material = Materials[0];
        
        transform.Translate(Vector3.up * .05f, Space.Self);
        checkKeyRelease();
    }
    private bool checkKeyHit(){
        foreach (Note i in noteManager.activenotes){
            if (i.starttime > Time.time - noteManager.notebuffer && i.starttime < Time.time + noteManager.notebuffer && i.noteID == keyID){
                Debug.Log("HIT: " + Time.time + " : " + i.starttime);
                thisKeysRenderer.material = Materials[2];
                i.activate();
                noteManager.pressednotes.Add(i);
                noteManager.activenotes.Remove(i);
                return true;
            }
        }
        Debug.Log("MISS"+ Time.time);
        return false;
    }
    private bool checkKeyRelease(){
        foreach (Note i in noteManager.pressednotes){
            if (i.endtime > Time.time - noteManager.notebuffer && i.endtime < Time.time + noteManager.notebuffer && i.noteID == keyID){
                Debug.Log("Rease HIT: " + Time.time + " : " + i.starttime);
                i.correct();
                noteManager.pressednotes.Remove(i);
                return true;   
            }
        }
        Debug.Log("MISS"+ Time.time);
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingKeyScript : Key
{
     public void KeyDown(int speed)
    {
        //Debug.Log("KeyDown called: " + keyID);
        
        Vector3 spawnPosition = transform.position - transform.forward * (transform.localScale.z / 2);

        thisKeysRenderer.material = Materials[1];
        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 0f, 0f);
        Note newNote = Instantiate(
                NotePrefab,spawnPosition,
                Quaternion.identity
            );
        newNote.transform.SetParent(this.transform);
        newNote.noteID = keyID;
        newNote.starttime = Time.time;
        
        currentnote = newNote;
        transform.Translate(Vector3.down * .05f, Space.Self);

        SoundManager.PlaySound(pianoSound, speed / 127f);

    }
    public void KeyUp()
    {
        //Debug.Log("KeyUp called: " + keyID);
        thisKeysRenderer.material = Materials[0];
        
        if (currentnote != null)
        {
            currentnote.endtime = Time.time;
            currentnote.on = false;
            currentnote = null;
        }
        transform.Translate(Vector3.up * .05f, Space.Self);
    }
}

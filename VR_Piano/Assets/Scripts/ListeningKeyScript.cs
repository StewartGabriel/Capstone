using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListeningKeyScript : Key
{
     public void KeyDown(int speed)
    {
        //Debug.Log("KeyDown called: " + keyID);
        
        thisKeysRenderer.material = Materials[1];
        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 0f, 0f);
        transform.Translate(Vector3.down * .05f, Space.Self);

        SoundManager.PlaySound(pianoSound, speed / 127f);

    }
    public void KeyUp()
    {
        //Debug.Log("KeyUp called: " + keyID);
        thisKeysRenderer.material = Materials[0];
        
        transform.Translate(Vector3.up * .05f, Space.Self);
    }
}

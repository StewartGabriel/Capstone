using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID; 
    public Renderer thisKeysRenderer;
    
    public Material[] Materials;
    public SoundType pianoSound;
    protected Note currentnote;
    protected bool isactive;
    public NoteManager noteManager;
    void Start()
    {
        thisKeysRenderer.material = Materials[0];
    } 
    void Update()
    {
        
    }
    public virtual void KeyDown(int speed)
    {
        thisKeysRenderer.material = Materials[1];
//        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 0f, 0f);
        transform.Translate(Vector3.down * .05f, Space.Self);
    }
    public virtual void KeyUp()
    {
        Debug.Log("KeyUp called: " + keyID);
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

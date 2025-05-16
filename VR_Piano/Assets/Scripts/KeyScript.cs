using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyID; 
    public Renderer thisKeysRenderer;
    
    public Material[] Materials;
    public Vector3[] KeyDimensions;
    public SoundType pianoSound;
    protected Note currentnote;
    public bool black;
    public NoteManager noteManager;

    public void Initiallize(bool isblack)
    {
        black = isblack;
        if (isblack)
        {
            transform.localScale = KeyDimensions[1];
            thisKeysRenderer.material = Materials[1];
        }
        else
        {
            transform.localScale = KeyDimensions[0];
            thisKeysRenderer.material = Materials[0];
        }
    }
    void Start()
    {
        thisKeysRenderer.material = Materials[0];
    } 

    void Update()
    {
        
    }
    public virtual void KeyDown(int speed, bool hand)
    {
        thisKeysRenderer.material = Materials[2];
//        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 0f, 0f);
        transform.Translate(Vector3.down * transform.lossyScale.y, Space.Self);

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
        transform.Translate(Vector3.up * transform.lossyScale.y, Space.Self);
    }
}

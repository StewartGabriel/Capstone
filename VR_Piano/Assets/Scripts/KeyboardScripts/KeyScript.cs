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
    // private float keyupheight;
    // private float keydownheight;
     

    public void Initiallize(bool isblack)
    {
        // keydownheight = transform.position.y;
        // keyupheight = keydownheight + transform.lossyScale.y;
        
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
        
    } 

    void Update()
    {
        
    }
    public virtual void KeyDown(int speed, bool hand)
    {
        thisKeysRenderer.material = Materials[2];
//        Debug.Log(speed);
        thisKeysRenderer.material.color = new Color(Mathf.Clamp01((1f / 381f) * speed + 0.5f), 0f, 0f);
        Vector3 newheight = transform.position;
        if (black)
        {
            newheight.y = transform.parent.position.y + transform.parent.lossyScale.y / 2 - transform.lossyScale.y / 2 + KeyDimensions[0].y;
        }
        else
        {
            newheight.y = transform.parent.position.y + transform.parent.lossyScale.y / 2 - transform.lossyScale.y / 2;
        }
            
        transform.position = newheight;
        Debug.Log("KeyDown called: " + keyID + ", " + speed + ", " + hand);
    }
    public virtual void KeyUp()
    {
        Debug.Log("KeyUp called: " + keyID);
         if(black)
            thisKeysRenderer.material = Materials[1];
        else
            thisKeysRenderer.material = Materials[0];

        
        Vector3 newheight = transform.position;
        if (black)
        {
            newheight.y = transform.parent.position.y + transform.parent.lossyScale.y / 2 + transform.lossyScale.y / 2 + KeyDimensions[0].y;
        }
        else
        {
            newheight.y = transform.parent.position.y + transform.parent.lossyScale.y / 2 + transform.lossyScale.y / 2;
        }
        transform.position = newheight;
    }
}

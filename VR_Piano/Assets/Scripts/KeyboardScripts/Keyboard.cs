using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// NoteCallback.cs - This script shows how to define a callback to get notified
// on MIDI note-on/off events.

public class PianoKeyboard : MonoBehaviour
{
    //public GameObject KeyboardBase;
    public Key[]KeySet;
    public Key KeyPreFab;
    public Key BlackKeyPreFab; 
    public int KeyCount;
    public float spacing;
    public int FirstNoteID;
    public NoteManager notemanager;
    protected List<float> fretlocations = new List<float>();
    
    public void Awake()
    {
        CreateBoard();
    }

    void Update()
    {
       
    }

  
    protected void CreateBoard()
    {
        KeySet = new Key[KeyCount];

        transform.localScale = new Vector3(transform.localScale.x, KeyPreFab.transform.localScale.y *2, KeyPreFab.transform.localScale.z);

        float width = transform.lossyScale.x;
        float height = transform.lossyScale.y;
        float keyDepth = KeyPreFab.transform.lossyScale.z;

        float keyWidth  = KeyPreFab.transform.lossyScale.x;

        float currentPosition = transform.position.x - width / 2 - keyWidth / 2;

        float yPosition = transform.position.y + height / 2;


        int[] blackwhitepattern = {1,0,1,1,0,1,0,1,1,0,1,0};
        int octavetracker  = 7;

        
        for (int i = 0; i < KeyCount; i++)
        {
            if (blackwhitepattern[octavetracker] == 1)
            {
                currentPosition += keyWidth + spacing;
                Key newKey = Instantiate(
                    KeyPreFab,
                    new Vector3(currentPosition, yPosition + KeyPreFab.transform.lossyScale.y / 2, transform.position.z),
                    Quaternion.identity
                );
                newKey.Initiallize(false);
                KeySet[i] = newKey;
                if (octavetracker == 3)
                fretlocations.Add(currentPosition - KeyPreFab.transform.lossyScale.x / 2 - spacing / 2);
   
            }
            else
            {
                float blackKeyOffset = (keyWidth + spacing) * 0.5f;
                Key blackKey = Instantiate(
                    KeyPreFab,
                    new Vector3(currentPosition + blackKeyOffset, yPosition + KeyPreFab.transform.lossyScale.y + KeyPreFab.transform.lossyScale.y / 2, this.transform.position.z + keyDepth * 1 / 5),//the math wasnt working out so the 1/5 value is a brute force solution
                    Quaternion.identity
                );
                blackKey.Initiallize(true);
                KeySet[i] = blackKey;
            }
            
            KeySet[i].keyID = i + FirstNoteID;
            KeySet[i].noteManager = notemanager;

            if (octavetracker == 11){
                octavetracker = 0;
            }
            else{
                octavetracker++;
            }
        }
        float firstKeyX = KeySet[0].transform.position.x;
        float lastKeyX = KeySet[KeySet.Length - 1].transform.position.x;
        float newboardsize = (lastKeyX - firstKeyX) + keyWidth; // Include last key's width
        float newboardcenter = firstKeyX - keyWidth/2 + newboardsize / 2;

        // Correct position centering
        this.transform.position = new Vector3(newboardcenter, transform.position.y, transform.position.z);

        // Adjust scale correctly
        Vector3 scale = transform.localScale;
        scale.x = newboardsize / transform.lossyScale.x; // Scale relative to original size
        transform.localScale = scale;

        for(int i  = 0; i< KeySet.Length; i++){
            KeySet[i].transform.SetParent(this.transform);
        }
    }

    
}
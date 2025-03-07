using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// NoteCallback.cs - This script shows how to define a callback to get notified
// on MIDI note-on/off events.

public sealed class NoteCallback : MonoBehaviour
{
    //public GameObject KeyboardBase;
    private Key[]KeySet;
    public Key KeyPreFab;
    public Key BlackKeyPreFab; 
    public int KeyCount;
    public float spacing;
    void Start()
    {
        CreateBoard();
    }

    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            KeySet[0].KeyDown(Random.Range(0, 128));
        }

        if (Keyboard.current.wKey.wasReleasedThisFrame)
        {
            KeySet[0].KeyUp();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            KeySet[1].KeyDown(Random.Range(0, 128));
        }

        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            KeySet[1].KeyUp();
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            KeySet[2].KeyDown(Random.Range(0, 128));
        }

        if (Keyboard.current.rKey.wasReleasedThisFrame)
        {
            KeySet[2].KeyUp();
        }
    }

  
    void CreateBoard()
    {
        KeySet = new Key[KeyCount];

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        float width = boxCollider.size.x * transform.localScale.x;
        float height = boxCollider.size.y * transform.localScale.y;
        float depth  = boxCollider.size.z * transform.localScale.z;
        
        float keyWidth = KeyPreFab.GetComponent<BoxCollider>().size.x * KeyPreFab.transform.lossyScale.x;
        float keyDepth = KeyPreFab.GetComponent<BoxCollider>().size.z * KeyPreFab.transform.lossyScale.z;
        float blackKeyDepth = BlackKeyPreFab.GetComponent<BoxCollider>().size.z * BlackKeyPreFab.transform.lossyScale.z;

        float currentPosition = transform.position.x - width / 2 - keyWidth / 2;

        float yPosition = transform.position.y + height / 2;


        int[] blackwhitepattern = {1,0,1,1,0,1,0,1,1,0,1,0};
        int octavetracker  = 3;

        for (int i = 0; i < KeyCount; i++)
        {
            if (blackwhitepattern[octavetracker] == 1){
                currentPosition += keyWidth + spacing;
                Key newKey = Instantiate(
                KeyPreFab,
                new Vector3(currentPosition, yPosition, transform.position.z),
                Quaternion.identity
                );
                KeySet[i] = newKey;
            }
            else
            {
                float blackKeyOffset = (keyWidth + spacing) * 0.5f;
                Key blackKey = Instantiate(
                    BlackKeyPreFab,
                    new Vector3(currentPosition + blackKeyOffset, yPosition + 0.1f, this.transform.position.z + keyDepth * 1/5),//the math wasnt working out so the 1/5 value is a brute force solution
                    Quaternion.identity
                );
                
                KeySet[i] = blackKey;
            }

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
        scale.x = newboardsize / boxCollider.size.x; // Scale relative to original size
        transform.localScale = scale;

        for(int i  = 0; i< KeySet.Length; i++){
            KeySet[i].transform.SetParent(this.transform);
        }
    }

    public void InterpretMidi(int note, int velocity)
    {
        Debug.Log("Note Received From Library: " + note);
        if (velocity > 0)
        {
            // Map each midi note to it's designated sound clip from pianoSounds
            float volume = velocity / 127.0f; // Volume
            SoundManager.PlaySound(SoundType.pianoSounds, note, volume);
            
            KeySet[note - 1].KeyDown(velocity);
        }
        else
        {
            KeySet[note - 1].KeyUp();
        }
    }
}
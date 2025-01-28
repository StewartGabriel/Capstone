<<<<<<< HEAD
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// NoteCallback.cs - This script shows how to define a callback to get notified
// on MIDI note-on/off events.

sealed class NoteCallback : MonoBehaviour
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

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) => {
                Debug.Log(string.Format(
                    "Note On #{0} ({1}) vel:{2:0.00} ch:{3} dev:'{4}'",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity,
                    (note.device as Minis.MidiDevice)?.channel,
                    note.device.description.product
                ));
                if(note.noteNumber < KeyCount){KeySet[note.noteNumber].KeyDown((int)velocity);}
            };

            midiDevice.onWillNoteOff += (note) => {
                Debug.Log(string.Format(
                    "Note Off #{0} ({1}) ch:{2} dev:'{3}'",
                    note.noteNumber,
                    note.shortDisplayName,
                    (note.device as Minis.MidiDevice)?.channel,
                    note.device.description.product
                ));
                if(note.noteNumber < KeyCount){KeySet[note.noteNumber].KeyUp();}
            };
        };
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

    float width = 1f;
    float height = 0f;
    BoxCollider boxCollider = GetComponent<BoxCollider>();
    width = boxCollider.size.x * transform.localScale.x;
    height = boxCollider.size.y * transform.localScale.y;

    float totalSpacing = (KeyCount - 1) * spacing;
    float keyWidth = (width - totalSpacing) / KeyCount;

    float currentPosition = transform.position.x - width / 2 + keyWidth / 2;

    for (int i = 0; i < KeyCount; i++)
    {
        float yPosition = transform.position.y + height / 2;

        Key newKey = Instantiate(
            KeyPreFab,
            new Vector3(currentPosition, yPosition, transform.position.z),
            Quaternion.identity
        );

        Vector3 whiteScale = newKey.transform.localScale;
        whiteScale.x = keyWidth / newKey.GetComponent<BoxCollider>().size.x;
        newKey.transform.localScale = whiteScale;

        KeySet[i] = newKey;

        if (i % 7 != 2 && i % 7 != 6)
        {
            float blackKeyOffset = keyWidth * 0.5f; 

            Key blackKey = Instantiate(
                BlackKeyPreFab,
                new Vector3(currentPosition + blackKeyOffset, yPosition + 0.1f, transform.position.z - whiteScale.z + transform.localScale.z), // Adjust y-position for black keys
                Quaternion.identity
            );

            Vector3 blackScale = blackKey.transform.localScale;
            blackScale.x = (keyWidth * 0.6f) / blackKey.GetComponent<BoxCollider>().size.x; // Black keys are narrower
            blackKey.transform.localScale = blackScale;
            
            i++;
            KeySet[i] = blackKey;
        }
        currentPosition += keyWidth + spacing;
    }
}

}
=======
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Controls;

public class NoteCallback : MonoBehaviour
{
    //public GameObject KeyboardBase;
    private Key[] KeySet;
    public Key KeyPreFab;
    public int KeyCount;
    public float spacing;

    // Keyboard keys dictionary
    private Dictionary<KeyControl, int> keyMappings;
    void Start()
    {
        CreateBoard();
        InitializeKeyMappings();
    }

    void InitializeKeyMappings() // For testing with a computer's keyboard rather than a midi keyboard
    {
        keyMappings = new Dictionary<KeyControl, int>
        {
            { Keyboard.current.qKey, 57},
            { Keyboard.current.wKey, 59},
            { Keyboard.current.eKey, 60},
            { Keyboard.current.rKey, 62},
            { Keyboard.current.tKey, 64},
            { Keyboard.current.yKey, 65},
            { Keyboard.current.uKey, 67},
            { Keyboard.current.iKey, 69},
            { Keyboard.current.oKey, 71},
            { Keyboard.current.pKey, 74},
            { Keyboard.current.aKey, 76},
            { Keyboard.current.sKey, 77},
            { Keyboard.current.dKey, 79},
            { Keyboard.current.fKey, 80},
            { Keyboard.current.gKey, 80},
            { Keyboard.current.hKey, 80},
            { Keyboard.current.jKey, 80},
            { Keyboard.current.kKey, 80},
            { Keyboard.current.lKey, 80},
            { Keyboard.current.zKey, 80},
            { Keyboard.current.xKey, 80},
            { Keyboard.current.cKey, 80},
            { Keyboard.current.vKey, 80},
            { Keyboard.current.bKey, 80},
        };
    }


    void Update() // For testing with a computer's keyboard rather than a midi keyboard
    {
        foreach (var keyMapping in keyMappings)
        {
            var key = keyMapping.Key;
            var index = keyMapping.Value;

            if (key.wasPressedThisFrame)
            {
                KeySet[index].KeyDown(Random.Range(0, 128));
            }

            if (key.wasReleasedThisFrame)
            {
                KeySet[index].KeyUp();
            }
        }
    }

    void CreateBoard() // Create the render of the keyboard
    {
        KeySet = new Key[KeyCount];

        float width = 1f;
        float height = 0f;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        width = boxCollider.size.x * transform.localScale.x;
        height = boxCollider.size.y * transform.localScale.y;


        float totalSpacing = (KeyCount - 1) * spacing;
        float keyWidth = (width - totalSpacing) / KeyCount;

        float leftEdge = transform.position.x - width / 2;

        for (int i = 0; i < KeyCount; i++)
        {
            float xPosition = leftEdge + keyWidth / 2 + i * (keyWidth + spacing);
            float yPosition = transform.position.y + height / 2;

            Key newKey = Instantiate(
                KeyPreFab,
                new Vector3(xPosition, yPosition, transform.position.z), // Adjust as needed
                Quaternion.identity
            );

            // Assign pianoSound from SoundType to each key
            if (i == 57 || i == 69)
            {
                newKey.pianoSound = SoundType.aNOTES; // aNotes - gNotes, to be replaced by enum nOctaves from SoundManager in future implementations
            }
            else if (i == 59 || i == 71)
            {
                newKey.pianoSound = SoundType.bNOTES;
            }
            else if (i == 60 || i == 72)
            {
                newKey.pianoSound = SoundType.cNOTES;
            }
            else if (i == 62 || i == 74)
            {
                newKey.pianoSound = SoundType.dNOTES;
            }
            else if (i == 64 || i == 76)
            {
                newKey.pianoSound = SoundType.eNOTES;
            }
            else if (i == 65 || i == 77)
            {
                newKey.pianoSound = SoundType.fNOTES;
            }
            else if (i == 67 || i == 79)
            {
                newKey.pianoSound = SoundType.gNOTES;
            }
            else if (i >= 80) //I'll figure this out later
            {
                newKey.pianoSound = (SoundType.SHARPS); // SHARPS
            }

            KeySet[i] = newKey;
            newKey.transform.SetParent(this.transform);
        }
    }

    public void InterpretMidi(int note, int velocity)
    {
        Debug.Log("Note Received From Library: " + note);
        if (velocity > 0)
        {
            KeySet[note - 1].KeyDown(velocity);
        }
        else
        {
            KeySet[note - 1].KeyUp();
        }
    }
}
>>>>>>> 581b53abe26c01b02467ebfcbfaf9da5254df5a0

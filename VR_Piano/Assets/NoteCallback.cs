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
    }
    void CreateBoard(){
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

            KeySet[i] = newKey;
            newKey.transform.SetParent(this.transform);
        }
    }
}

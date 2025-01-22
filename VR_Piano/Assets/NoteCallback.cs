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
            KeySet[2].KeyDown(Random.Range(0, 128));
        }

        if (Keyboard.current.eKey.wasReleasedThisFrame)
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
                new Vector3(currentPosition + blackKeyOffset, yPosition + 0.1f, transform.position.z), // Adjust y-position for black keys
                Quaternion.identity
            );

            Vector3 blackScale = blackKey.transform.localScale;
            blackScale.x = (keyWidth * 0.6f) / blackKey.GetComponent<BoxCollider>().size.x; // Black keys are narrower
            blackKey.transform.localScale = blackScale;
            
            i++;
        }
        currentPosition += keyWidth + spacing;
    }
}

}

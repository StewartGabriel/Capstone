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
            newKey.transform.SetParent(this.transform);
            
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
                    blackKey.transform.SetParent(this.transform);
            
            }
            currentPosition += keyWidth + spacing;

            // // Assign pianoSound from SoundType to each key
            //if (i >= 22 || i <= 109)
            //{
            //     newKey.pianoSound = SoundType.pianoSounds;
            //}
            // KeySet[i] = newKey;
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
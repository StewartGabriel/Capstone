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

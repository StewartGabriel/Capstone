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
            { Keyboard.current.qKey, 0},
            { Keyboard.current.wKey, 1},
            { Keyboard.current.eKey, 2},
            { Keyboard.current.rKey, 3},
            { Keyboard.current.tKey, 4},
            { Keyboard.current.yKey, 5},
            { Keyboard.current.uKey, 6},
            { Keyboard.current.iKey, 7},
            { Keyboard.current.oKey, 8},
            { Keyboard.current.pKey, 9},
            { Keyboard.current.aKey, 10},
            { Keyboard.current.sKey, 11},
            { Keyboard.current.dKey, 12},
            { Keyboard.current.fKey, 13},
            { Keyboard.current.gKey, 14},
            { Keyboard.current.hKey, 15},
            { Keyboard.current.jKey, 16},
            { Keyboard.current.kKey, 17},
            { Keyboard.current.lKey, 18},
            { Keyboard.current.zKey, 19},
            { Keyboard.current.xKey, 20},
            { Keyboard.current.cKey, 21},
            { Keyboard.current.vKey, 22},
            { Keyboard.current.bKey, 23},
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
            if (i < 2)
            {
                newKey.pianoSound = SoundType.aNOTES; // aNotes - gNotes, to be replaced by enum nOctaves from SoundManager in future implementations
            }
            else if (i < 4)
            {
                newKey.pianoSound = SoundType.bNOTES;
            }
            else if (i < 6)
            {
                newKey.pianoSound = SoundType.cNOTES;
            }
            else if (i < 8)
            {
                newKey.pianoSound = SoundType.dNOTES;
            }
            else if (i < 10)
            {
                newKey.pianoSound = SoundType.eNOTES;
            }
            else if (i < 12)
            {
                newKey.pianoSound = SoundType.fNOTES;
            }
            else if (i < 14)
            {
                newKey.pianoSound = SoundType.gNOTES;
            }
            else if (i < 24)
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

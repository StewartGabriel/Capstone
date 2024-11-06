using UnityEngine;
using MidiJack;
using System.Collections.Generic;
using System.Diagnostics;

public class MidiInputHandler : MonoBehaviour
{
    private Dictionary<int, AudioClip> noteToSound;

    void Start()
    {
        noteToSound = new Dictionary<int, AudioClip>();
        LoadSounds();

        MidiDriver.Instance.NoteOnDelegate += NoteOn;
    }

    void LoadSounds()
    {
        for (int note = 0; note < 128; note++) // MIDI notes range from 0 to 127
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/{note}");
            if (clip != null)
            {
                noteToSound[note] = clip;
            }
        }
    }

    void NoteOn(MidiChannel channel, int note, float velocity)
    {
        Debug.Log($"Note On: Channel {channel}, Note {note}, Velocity {velocity}");

        if (noteToSound.ContainsKey(note))
        {
            AudioSource.PlayClipAtPoint(noteToSound[note], Vector3.zero);
        }
    }

    void OnDestroy()
    {
        MidiDriver.Instance.NoteOnDelegate -= NoteOn;
    }
}

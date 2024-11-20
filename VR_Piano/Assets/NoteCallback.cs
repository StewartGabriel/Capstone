using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using System.Collections.Generic;
using Minis;

sealed class NoteCallback : MonoBehaviour
{
    private Dictionary<int, AudioClip> midiNoteToSound;

    void Start()
    {
        midiNoteToSound = new Dictionary<int, AudioClip>();
        LoadSounds();

        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                Debug.Log($"Note On #{note.noteNumber} vel:{velocity:0.00} ch:{midiDevice.channel} dev:'{note.device.description.product}'");

                if (midiNoteToSound.TryGetValue(note.noteNumber, out var clip))
                {
                    AudioSource.PlayClipAtPoint(clip, Vector3.zero);
                }
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                Debug.Log($"Note Off #{note.noteNumber} ch:{midiDevice.channel} dev:'{note.device.description.product}'");

                // Stop sound logic can be added here
            };
        };
    }

    void LoadSounds()
    {
        // Load audio clips and map them to corresponding MIDI notes
        midiNoteToSound[48] = Resources.Load<AudioClip>("c3"); // C3
        // Continue loading other notes...
    }
}

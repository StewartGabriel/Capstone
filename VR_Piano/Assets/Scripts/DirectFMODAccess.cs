using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DirectFMODAccess : MonoBehaviour
{
    // This function plays the FMOD Piano event directly
    public void PlayPianoEvent(int note, float volume = 1)
    {
        Debug.Log($"Playing FMOD Piano Event - Note: {note}, Volume: {volume}");

        // Create the FMOD EventInstance for the Piano event
        EventInstance pianoEvent = RuntimeManager.CreateInstance("event:/Piano");

        // Set the FMOD parameters
        pianoEvent.setParameterByName("Note", note);    // Set the MIDI note parameter
        pianoEvent.setParameterByName("Volume", volume); // Set velocity as volume

        // Start the FMOD event and release it after playing
        pianoEvent.start();
        pianoEvent.release(); // Clean up after the event starts
    }
}
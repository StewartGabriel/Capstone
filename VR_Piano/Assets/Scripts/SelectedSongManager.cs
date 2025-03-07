using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlaySceneManager : MonoBehaviour
{
    private int selectedSongIndex;
    [SerializeField] private MidiMessages midiMessages; // create reference to MidiMesseges component

    void Start()
    {
        // Retrieve the stored song index (default to 1 if not found)
        selectedSongIndex = PlayerPrefs.GetInt("SelectedSong", 1);
        Debug.Log("Now playing song number: " + selectedSongIndex);

        // Call function to play the selected song
        PlaySelectedSong();
    }

    private void PlaySelectedSong()
    {
        // Checks that MidiMessages component is assigned
        if (midiMessages != null)
        {
            await Task.Delay(3000); // Waits for 2000 milliseconds (3 seconds)
            // Plau the song selection from MidiMessages
            midiMessages.PlaySong(selectedSongIndex);
        }
        else
        {
            Debug.LogError("MidiMesseges component not assigned.");
        }

        // Debug.Log("Playing song with index: " + selectedSongIndex);
    }
}
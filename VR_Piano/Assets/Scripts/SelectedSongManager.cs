using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour
{
    private int selectedSongIndex;

    void Start()
    {
        // Retrieve the stored song index (default to 1 if not found)
        selectedSongIndex = PlayerPrefs.GetInt("SelectedSongIndex", 1);
        Debug.Log("Now playing song number: " + selectedSongIndex);

        // Call function to play the selected song
        PlaySelectedSong();
    }

    private void PlaySelectedSong()
    {
        // Replace with actual logic to play a song from a list
        Debug.Log("Playing song with index: " + selectedSongIndex);
    }
}
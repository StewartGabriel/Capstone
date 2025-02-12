using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlaySceneSceneSwitcher : MonoBehaviour
{
    private string playScene = "PlayScene";
    private int selectedSong = 1; // Default song

    private void Awake()
    {
        // Ensure playScene is not null
        if (!string.IsNullOrEmpty(playScene))
        {
            Debug.Log($"Play Scene set to {playScene}");
        }
        else
        {
            Debug.LogWarning("Play Scene name is empty!");
        }
    }

    // Called by UI buttons when clicked
    public void OnSongButtonClicked(GameObject button)
    {
        int songNumber = ExtractSongNumber(button.name);
        if (songNumber > 0)
        {
            SetSelectedSong(songNumber);
        }
        else
        {
            Debug.LogWarning($"Could not determine song number from {button.name}");
        }
    }

    // Extracts the song number from button names like "Song3Button".
    private int ExtractSongNumber(string buttonName)
    {
        Match match = Regex.Match(buttonName, @"\d+");
        return match.Success && int.TryParse(match.Value, out int number) ? number : -1;
    }

    // Centralized method for setting the selected song
    private void SetSelectedSong(int songNumber)
    {
        selectedSong = songNumber;
        PlayerPrefs.SetInt("SelectedSong", selectedSong);
        PlayerPrefs.Save();
        Debug.Log($"Song {selectedSong} selected. Loading PlayScene...");
        SwitchToPlayScene();
    }

    // Loads the play scene asynchronously while keeping the selected song.
    public void SwitchToPlayScene()
    {
        if (!string.IsNullOrEmpty(playScene))
        {
            StartCoroutine(LoadSceneAsync(playScene));
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Make sure it is correctly assigned.");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Retrieves the selected song in PlayScene.
    public static int GetSelectedSong()
    {
        return PlayerPrefs.GetInt("SelectedSong", 1); // Defaults to Song 1 if none is selected
    }
}

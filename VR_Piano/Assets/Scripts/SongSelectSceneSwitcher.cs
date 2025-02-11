using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Text.RegularExpressions;

public class PlaySceneSceneSwitcher : MonoBehaviour
{
    private string playScene = "PlayScene";
    private int selectedSong = 1; // Default song

    [SerializeField] private InputActionReference leftHandAction;  // XR Left Hand Input
    [SerializeField] private InputActionReference rightHandAction; // XR Right Hand Input

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

    private void OnEnable()
    {
        // Enable XR input actions
        leftHandAction.action.Enable();
        rightHandAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable XR input actions
        leftHandAction.action.Disable();
        rightHandAction.action.Disable();
    }

    private void Update()
    {
        // Allow scene switching via XR input
        if (IsButtonPressed(leftHandAction) || IsButtonPressed(rightHandAction))
        {
            SwitchToPlayScene();
        }
    }

    private bool IsButtonPressed(InputActionReference actionReference)
    {
        // Check if an action is performed
        return actionReference.action != null && actionReference.action.triggered;
    }

    
    // Called by UI buttons. Extracts song number from the button name.
    public void SelectSongFromButton(GameObject button)
    {
        int songNumber = ExtractSongNumber(button.name);
        if (songNumber > 0)
        {
            selectedSong = songNumber;
            PlayerPrefs.SetInt("SelectedSong", selectedSong);
            PlayerPrefs.Save();
            Debug.Log($"Song {selectedSong} selected. Loading PlayScene...");
            SwitchToPlayScene();
        }
        else
        {
            Debug.LogWarning($"Could not determine song number from {button.name}");
        }
    }

    // Extracts the song number from button names like "Song3Button".
    private int ExtractSongNumber(string buttonName)
    {
        string numberString = Regex.Match(buttonName, @"\d+").Value;
        return int.TryParse(numberString, out int number) ? number : -1;
    }

    // Loads the play scene while keeping the selected song.
    public void SwitchToPlayScene()
    {
        if (!string.IsNullOrEmpty(playScene))
        {
            SceneManager.LoadScene(playScene);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Make sure it is correctly assigned.");
        }
    }

    // Retrieves the selected song in PlayScene.
    public static int GetSelectedSong()
    {
        return PlayerPrefs.GetInt("SelectedSong", 1); // Defaults to Song 1 if none is selected
    }
}

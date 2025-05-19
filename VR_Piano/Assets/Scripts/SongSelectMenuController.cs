using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SongSelectMenuController : MonoBehaviour
{
    [SerializeField] private GameObject songSelectMenu; // Reference to the Song Select Menu panel
    [SerializeField] private Button[] songButtons; // Array of 16 song buttons
    [SerializeField] private Button backButton; // Back button to return to StartMenu
    [SerializeField] private string sampleSceneName; // Scene where the songs play
    [SerializeField] private string startScreenSceneName; // Name of the start screen scene

    // XR Hand Inputs for pinch gestures (same method as pause menu)
    [SerializeField] private InputActionReference leftHandSelectAction;  // Left hand pinch gesture
    [SerializeField] private InputActionReference rightHandSelectAction; // Right hand pinch gesture
    [SerializeField] private Transform leftHandTransform; // Left hand position reference
    [SerializeField] private Transform rightHandTransform; // Right hand position reference
    [SerializeField] private float selectionRadius = 0.05f; // Radius for detecting button selection

    // For the Play Song Button    
    [SerializeField] private Button playButton;
    private int selectedSongNumber = -1; // No song selected by default

    [SerializeField] private Toggle leftHandToggle;
    [SerializeField] private Toggle rightHandToggle;
    [SerializeField] private Slider tempoSlider;

    // Keyboard Config
    [SerializeField] private Button keyboardConfigButton;
    [SerializeField] private string keyboardConfigSceneName; 

    // Song Info Display 
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text composerText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text TempoText;
    private Dictionary<int, Song> songInfo = new Dictionary<int, Song>();

    private void OnEnable()
    {
        if (leftHandSelectAction?.action != null)
        {
            leftHandSelectAction.action.Enable();
            leftHandSelectAction.action.performed += OnLeftHandSelect;
        }

        if (rightHandSelectAction?.action != null)
        {
            rightHandSelectAction.action.Enable();
            rightHandSelectAction.action.performed += OnRightHandSelect;
        }
    }

    private void OnDisable()
    {
        if (leftHandSelectAction?.action != null)
        {
            leftHandSelectAction.action.performed -= OnLeftHandSelect;
            leftHandSelectAction.action.Disable();
        }

        if (rightHandSelectAction?.action != null)
        {
            rightHandSelectAction.action.performed -= OnRightHandSelect;
            rightHandSelectAction.action.Disable();
        }
    }

    private void Start()
    {

        if (tempoSlider != null)
        {
            tempoSlider.onValueChanged.AddListener(UpdateTempoDisplay);
            UpdateTempoDisplay(tempoSlider.value); // Show initial value
        }


        songInfo[1] = new Song("Octave Test", "Composer A", "Used to test hand input.");
        songInfo[2] = new Song("Ode to Joy", "Ludwig Van Beethoven", "Originally written as the ode “An die Freude” (1785) by German playwright and historian Friedrich Schiller, Ode to Joy was later adapted into a musical piece and immortalized by Ludwig van Beethoven. It appears as the choral finale—the 4th and final movement—of his Symphony No. 9, composed between 1822 and 1824. The symphony was first performed in Vienna on May 7, 1824.");
        // SongInfo[3] = new Song("", "", "")
        // SongInfo[4] = new Song("", "", "")
        // SongInfo[5] = new Song("", "", "")
        // SongInfo[6] = new Song("", "", "")
        // SongInfo[7] = new Song("", "", "")
        // SongInfo[8] = new Song("", "", "")
        // SongInfo[9] = new Song("", "", "")
        // SongInfo[10] = new Song("", "", "")
        // SongInfo[11] = new Song("", "", "")
        // SongInfo[12] = new Song("", "", "")
        // SongInfo[13] = new Song("", "", "")
        // SongInfo[14] = new Song("", "", "")
        // SongInfo[15] = new Song("", "", "")
        // SongInfo[16] = new Song("", "", "")

        if (songSelectMenu == null)
        {
            Debug.LogError("Song Select Menu is not assigned.");
            return;
        }

        if (songButtons.Length != 16)
        {
            Debug.LogError("Expected 16 song buttons, but found " + songButtons.Length);
            return;
        }

        if (backButton == null)
        {
            Debug.LogError("Back button is not assigned.");
            return;
        }

        if (keyboardConfigButton == null)
        {
            Debug.LogError("Keyboard Config Button is not assigned.");
            return;
        }

        // Assign button listeners dynamically
        for (int i = 0; i < songButtons.Length; i++)
        {
            int songNumber = i + 1; // Song numbers start from 1
            songButtons[i].onClick.AddListener(() => SelectSong(songNumber));
        }

        if (playButton == null)
        {
            Debug.LogError("Play button is not assigned.");
            return;
        }

        playButton.onClick.AddListener(PlaySelectedSong);

        backButton.onClick.AddListener(ReturnToMainMenu);
        
        keyboardConfigButton.onClick.AddListener(OpenKeyboardConfig);
    }

   private void UpdateTempoDisplay(float value)
    {
        if (TempoText != null)
        {
            TempoText.text = value.ToString("0.0") + "x";  // Shows 1 decimal place
        }
    }


    [System.Serializable]
    public class Song
    {
        public string title;
        public string composer;
        public string description;

        public Song(string title, string composer, string description)
        {
            this.title = title;
            this.composer = composer;
            this.description = description;
        }
    }

    private void OnLeftHandSelect(InputAction.CallbackContext context)
    {
        SelectClosestButton(leftHandTransform);
    }

    private void OnRightHandSelect(InputAction.CallbackContext context)
    {
        SelectClosestButton(rightHandTransform);
    }

    private void SelectClosestButton(Transform handTransform)
    {
        if (handTransform == null)
        {
            Debug.LogWarning("Hand Transform is not assigned.");
            return;
        }

        Button closestButton = null;
        float closestDistance = float.MaxValue;

        foreach (Button button in songButtons)
        {
            if (button == null) continue;

            float distance = Vector3.Distance(handTransform.position, button.transform.position);
            if (distance < closestDistance && distance <= selectionRadius)
            {
                closestButton = button;
                closestDistance = distance;
            }
        }

        if (closestButton != null)
        {
            closestButton.onClick.Invoke();
            Debug.Log("Selected: " + closestButton.name);
        }
        else
        {
            Debug.Log("No button selected. Move closer and try again.");
        }
    }

    private void SelectSong(int songNumber)
    {
        Debug.Log("Loading song " + songNumber);
        selectedSongNumber = songNumber;
        Debug.Log("Selected song " + songNumber);

        //Highlight the selected song for the user
        HighlightSelectedButton(songNumber);

          if (songInfo.TryGetValue(songNumber, out Song song))
        {
            titleText.text = song.title;
            composerText.text = song.composer;
            descriptionText.text = song.description;
        }

        else
        {
            titleText.text = "Unknown Title";
            composerText.text = "Unknown Composer";
            descriptionText.text = "No description available.";
        }

    }

    private void PlaySelectedSong()
    {
        if (selectedSongNumber < 1)
        {
            Debug.LogWarning("No song selected.");
            return;
        }

        Debug.Log("Playing selected song " + selectedSongNumber);
        PlayerPrefs.SetInt("SelectedSong", selectedSongNumber);

        // Read from toggles and slider
        bool left_enabled = leftHandToggle != null && leftHandToggle.isOn;
        bool right_enabled = rightHandToggle != null && rightHandToggle.isOn;
        float tempo_multiplier = tempoSlider != null ? Mathf.RoundToInt(tempoSlider.value) : 1;

        PlayerPrefs.SetInt("LeftEnabled", left_enabled ? 1 : 0);
        PlayerPrefs.SetInt("RightEnabled", right_enabled ? 1 : 0);
        PlayerPrefs.SetFloat("TempoMultiplier", tempo_multiplier);

        PlayerPrefs.Save();

        SceneManager.LoadScene(sampleSceneName);
    }


    private void HighlightSelectedButton(int songNumber)
    {
        for (int i = 0; i < songButtons.Length; i++)
        {
            ColorBlock colors = songButtons[i].colors;
            colors.normalColor = (i + 1 == songNumber) ? Color.yellow : Color.white;
            songButtons[i].colors = colors;
        }
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Start Menu.");
        SceneManager.LoadScene(startScreenSceneName, LoadSceneMode.Single);
    }


    private void OpenKeyboardConfig()
    {
        Debug.Log("Switching to Keyboard Config scene.");
        SceneManager.LoadScene(keyboardConfigSceneName, LoadSceneMode.Single);
    }


}

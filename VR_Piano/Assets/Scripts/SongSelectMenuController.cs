using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

        // Assign button listeners dynamically
        for (int i = 0; i < songButtons.Length; i++)
        {
            int songNumber = i + 1; // Song numbers start from 1
            songButtons[i].onClick.AddListener(() => SelectSong(songNumber));
        }

        backButton.onClick.AddListener(ReturnToMainMenu);
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
        
        // Store the song number to be accessed in the next scene
        PlayerPrefs.SetInt("SelectedSong", songNumber);
        PlayerPrefs.Save();

        // Load the sample scene
        SceneManager.LoadScene(sampleSceneName);
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Start Menu.");
        SceneManager.LoadScene(startScreenSceneName, LoadSceneMode.Single);
    }


}

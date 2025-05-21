using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SongEndPanelController : MonoBehaviour
{

    [SerializeField] private GameObject songSelectMenu; // Reference to the Song Select Menu panel
    [SerializeField] private Button backButton; // Back button to return to StartMenu
    [SerializeField] private string startScreenSceneName; // Name of the start screen scene

    // XR Hand Inputs for pinch gestures
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

        if (backButton == null)
        {
            Debug.LogError("Back button is not assigned.");
            return;
        }
        songSelectMenu.SetActive(false);
        backButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void ToggleEndPanel()
    {
        if (songSelectMenu != null)
        {
            bool isActive = songSelectMenu.activeSelf;

            if (!isActive)
            {
                songSelectMenu.SetActive(true);
                Debug.Log("End Panel opened.");
            }
            else
            {
                Debug.Log("End Panel closed.");
            }
        }
        else
        {
            Debug.LogWarning("End Panel is not assigned in the Inspector.");
            return;
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

    private void ReturnToMainMenu()
    {
        Debug.Log("Returning to Start Menu.");
        SceneManager.LoadScene(startScreenSceneName, LoadSceneMode.Single);
    }

}

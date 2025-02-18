using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private string songSelectSceneName = "SongSelectV2";
    //[SerializeField] private string optionsSceneName = "MainOptions"; // Uncomment when implementing options

    // XR Hand Inputs for pinch gestures
    [SerializeField] private InputActionReference leftHandSelectAction;
    [SerializeField] private InputActionReference rightHandSelectAction;

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
        if (playButton == null || quitButton == null)
        {
            Debug.LogError("One or more buttons are not assigned.");
            return;
        }

        playButton.onClick.AddListener(StartGame);
        //optionsButton.onClick.AddListener(OpenOptions); // Uncomment when implementing options
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnLeftHandSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Left Hand Pinch Detected!");
        TriggerButtonClick();
    }

    private void OnRightHandSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Right Hand Pinch Detected!");
        TriggerButtonClick();
    }

    private void TriggerButtonClick()
    {
        // Assuming Unity XR UI system is handling the actual button interactions
        Debug.Log("XR Hand Input Triggered - UI System should handle button press.");
    }

    public void StartGame()
    {
        Debug.Log("Loading Song Select Scene...");
        SceneManager.LoadScene(songSelectSceneName);
    }

    /*
    public void OpenOptions()
    {
        Debug.Log("Opening Options Menu...");
        SceneManager.LoadScene(optionsSceneName);
    }
    */

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

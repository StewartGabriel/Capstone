using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; // Reference to the settings menu GameObject
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu GameObject
    [SerializeField] private GameObject calibrationSettings; // Reference to the calibrationSettings GameObject
    [SerializeField] private GameObject deviceSetup; // Reference to the deviceSetup GameObject
    [SerializeField] private InputActionReference wristMenuAction; // Wrist menu gesture input action reference
    [SerializeField] private Transform wristTransform; // Transform representing the user's wrist
    [SerializeField] private Transform centerScreenTransform; // Transform representing the center of the user's screen
    [SerializeField] private string startScreenSceneName = "StartMenu"; // Name of the start screen scene
    [SerializeField] private InputActionReference leftHandAction;  // Reference to the left hand action
    [SerializeField] private InputActionReference rightHandAction; // Reference to the right hand action
    [SerializeField] private Vector3 wristMenuOffset = new Vector3(0, 0.2f, 0); // Offset above the wrist (default: 20 cm above)
    [SerializeField] private Vector3 wristMenuRotationOffset = new Vector3(0, 0, 0); // Rotation offset for the menu in degrees

    private GameObject currentActiveMenu; // Tracks the currently active menu

    private void OnEnable()
    {
        if (wristMenuAction?.action != null)
        {
            wristMenuAction.action.Enable();
            wristMenuAction.action.performed += OnWristMenuGesturePerformed;
        }
    }

    private void OnDisable()
    {
        if (wristMenuAction?.action != null)
        {
            wristMenuAction.action.performed -= OnWristMenuGesturePerformed;
            wristMenuAction.action.Disable();
        }
    }

    private void Start()
    {
        // Ensure all menus are inactive at the start
        CloseAllMenus();
    }

    private void Update()
    {
        // Keep the pause menu locked to the wrist if it is active
        if (pauseMenu != null && pauseMenu.activeSelf)
        {
            PositionMenuAboveWrist(pauseMenu);
        }
    }

    private void OnWristMenuGesturePerformed(InputAction.CallbackContext context)
    {
        TogglePauseMenu(); // Trigger the pause menu when the wrist menu gesture is detected
    }

    private void TogglePauseMenu()
    {
        if (pauseMenu != null)
        {
            bool isActive = pauseMenu.activeSelf;

            if (!isActive)
            {
                CloseAllMenusExcept(pauseMenu);
                PositionMenuAboveWrist(pauseMenu); // Position pause menu above wrist
                pauseMenu.SetActive(true);
                currentActiveMenu = pauseMenu;
                Debug.Log("Pause menu opened.");
            }
            else
            {
                CloseAllMenus(); // Close all menus when toggling off
                Debug.Log("Pause menu closed.");
            }
        }
        else
        {
            Debug.LogWarning("Pause menu is not assigned in the Inspector.");
        }
    }

    public void OpenSettingsMenu()
    {
        OpenMenu(settingsMenu, "Settings menu opened.");
    }

    public void OpenCalibrationSettings()
    {
        OpenMenu(calibrationSettings, "Calibration settings opened.");
    }

    public void OpenDeviceSetup()
    {
        OpenMenu(deviceSetup, "Device setup opened.");
    }

    private void OpenMenu(GameObject menu, string logMessage)
    {
        if (menu != null)
        {
            CloseAllMenusExcept(menu); // Close other menus
            PositionMenu(menu, centerScreenTransform); // Lock menu to the center of the screen
            menu.SetActive(true);
            currentActiveMenu = menu;
            Debug.Log(logMessage);
        }
        else
        {
            Debug.LogWarning("Menu is not assigned in the Inspector.");
        }
    }

    public void BackToPauseMenu()
    {
    CloseAllMenus();
    pauseMenu.SetActive(true); // Reopen the pause menu
    Debug.Log("Returned to Pause Menu.");
    }

    public void RestartLevel()
    {
    CloseAllMenus();
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    Debug.Log("Level restarted.");
    }

    public void ResumeGame()
    {
        CloseAllMenus(); // Close the pause menu and any active menus
        Debug.Log("Game resumed.");
    }

    public void GoToMainMenu()
    {
        CloseAllMenus(); // Ensure all menus are closed before switching scenes
        Debug.Log("Switching to main menu.");
        SceneManager.LoadScene(startScreenSceneName); // Load the main menu scene
    }

    private void PositionMenu(GameObject menu, Transform targetTransform)
    {
        if (menu != null && targetTransform != null)
        {
            menu.transform.position = targetTransform.position;
            menu.transform.rotation = targetTransform.rotation;
        }
    }

    private void PositionMenuAboveWrist(GameObject menu)
    {
        if (menu != null && wristTransform != null)
        {
            // Set the position on the open-hand side by using the forward vector
            Vector3 position = wristTransform.position +
                               wristTransform.forward * wristMenuOffset.z +  // Forward (above the palm side)
                               wristTransform.up * wristMenuOffset.y +       // Height above the wrist
                               wristTransform.right * wristMenuOffset.x;     // Left/right offset

            // Apply position
            menu.transform.position = position;

            // Apply rotation with offset
            Quaternion wristRotation = wristTransform.rotation;
            Quaternion rotationOffset = Quaternion.Euler(wristMenuRotationOffset); // Convert offset to Quaternion
            menu.transform.rotation = wristRotation * rotationOffset; // Combine wrist rotation with offset
        }
        else
        {
            Debug.LogWarning("Wrist Transform or menu is not assigned.");
        }
    }

    private void CloseAllMenusExcept(GameObject exception)
    {
        // Deactivate all menus except the specified one
        if (pauseMenu != exception) pauseMenu?.SetActive(false);
        if (settingsMenu != exception) settingsMenu?.SetActive(false);
        if (calibrationSettings != exception) calibrationSettings?.SetActive(false);
        if (deviceSetup != exception) deviceSetup?.SetActive(false);
    }

    public void CloseAllMenus()
    {
        // Disable all menus
        pauseMenu?.SetActive(false);
        settingsMenu?.SetActive(false);
        calibrationSettings?.SetActive(false);
        deviceSetup?.SetActive(false);
        currentActiveMenu = null;
        Debug.Log("All menus closed.");
    }
}

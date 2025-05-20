using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// For Audio Selection:
using UnityEngine.UI;
using FMODUnity;


public class StartPauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; // Reference to the settings menu GameObject
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu GameObject
    [SerializeField] private GameObject calibrationSettings; // Reference to the calibrationSettings GameObject
    [SerializeField] private GameObject deviceSetup; // Reference to the deviceSetup GameObject
    [SerializeField] private InputActionReference wristMenuAction; // Wrist menu gesture input action reference
    [SerializeField] private Transform wristTransform; // Transform representing the user's wrist
    [SerializeField] private Transform centerScreenTransform; // Transform representing the center of the user's screen
    //[SerializeField] private string startScreenSceneName = "StartMenu"; // Name of the start screen scene
    [SerializeField] private InputActionReference leftHandAction;  // Reference to the left hand action
    [SerializeField] private InputActionReference rightHandAction; // Reference to the right hand action
    [SerializeField] private Vector3 wristMenuOffset = new Vector3(0, 0.2f, 0); // Offset above the wrist (default: 20 cm above)
    [SerializeField] private Vector3 wristMenuRotationOffset = new Vector3(0, 0, 0); // Rotation offset for the menu in degrees

// For Audio Selection
    [SerializeField] private GameObject LogAudioDevices;
    private OculusFMODInitializer audioHandler;
    private OculusFMODCallbackHandler fMODCallbackHandler;
    
    [SerializeField] private Dropdown audioDeviceDropdown; 

    private GameObject currentActiveMenu;

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
        CloseAllMenus();

        // Get the OculusFMODInitializer component from LogAudioDevices
        if (LogAudioDevices != null)
        {
            audioHandler = LogAudioDevices.GetComponent<OculusFMODInitializer>();

            if (audioHandler == null)
            {
                Debug.LogError("OculusFMODInitializer component is missing on LogAudioDevices!");
            }
            else
            {
                Debug.Log("Initializer was successfully assigned.");
                fMODCallbackHandler = audioHandler.oculusFMODCallbackHandler;
            }
        }
        else
        {
            Debug.LogError("LogAudioDevices not assigned in Inspector.");
        }

        if (fMODCallbackHandler == null)
        {
            Debug.LogError("OculusFMODCallbackHandler ScriptableObject not assigned!");
            return;
        }

        PopulateAudioDeviceDropdown();

        // Retrieve the last used audio device to be used again
        string lastSelectedDevice = PlayerPrefs.GetString("SelectedAudioDevice", "");
        if (!string.IsNullOrEmpty(lastSelectedDevice))
        {
            List<string> deviceNames = new List<string>(fMODCallbackHandler.getAudioDrivers().Keys);
            int index = deviceNames.IndexOf(lastSelectedDevice);
            if (index >= 0)
            {
                StartCoroutine(SetDropdownValue(index));
                OnAudioDeviceSelected(index);
            }
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
            return;
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
        PopulateAudioDeviceDropdown(); // Refresh List of devices
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

    // public void GoToMainMenu()
    // {
    //     CloseAllMenus(); // Ensure all menus are closed before switching scenes
    //     Debug.Log("Switching to main menu.");
    //     SceneManager.LoadScene(startScreenSceneName); // Load the main menu scene
    // }

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
            // Position the menu near the wrist
            Vector3 position = wristTransform.position +
                            wristTransform.forward * wristMenuOffset.z +
                            wristTransform.up * wristMenuOffset.y +
                            wristTransform.right * wristMenuOffset.x;

            menu.transform.position = position;

            // Make the menu face the user and stay upright
            Vector3 forward = wristTransform.forward;
            forward.y = 0;
            forward.Normalize();

            if (forward == Vector3.zero)
                forward = Vector3.forward;

            Quaternion uprightRotation = Quaternion.LookRotation(forward, Vector3.up);
            menu.transform.rotation = uprightRotation * Quaternion.Euler(wristMenuRotationOffset + new Vector3(180, 180, 0));
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

    private void PopulateAudioDeviceDropdown()
    {
        if (audioDeviceDropdown == null)
        {
            Debug.LogError("Audio Device Dropdown is not assigned");
            return;
        }

        if (fMODCallbackHandler == null)
        {
            Debug.LogError("Oculus FMOD callback handler not assigned");
            return;
        }

        Dictionary<string, int> audioDrivers = fMODCallbackHandler.getAudioDrivers();
        audioDeviceDropdown.onValueChanged.RemoveAllListeners();
        audioDeviceDropdown.ClearOptions();
        List<string> deviceNames = new List<string>(audioDrivers.Keys);

        Debug.Log("Listing device names into the dropdown menu");
        foreach (string key in deviceNames)
        {
            Debug.Log("Inserting " + key);
        }

        audioDeviceDropdown.AddOptions(deviceNames);
        audioDeviceDropdown.onValueChanged.AddListener(delegate { OnAudioDeviceSelected(audioDeviceDropdown.value); });
    }

    private void OnAudioDeviceSelected(int index)
    {
        if (fMODCallbackHandler == null) return;

        Dictionary<string, int> audioDrivers = fMODCallbackHandler.getAudioDrivers();
        List<string> deviceNames = new List<string>(audioDrivers.Keys);

        if (index >= 0 && index < deviceNames.Count)
        {
            string selectedDevice = deviceNames[index];
            int deviceIndex = audioDrivers[selectedDevice];

            PlayerPrefs.SetString("SelectedAudioDevice", selectedDevice);
            PlayerPrefs.Save();

            setAudioOutputDevice(deviceIndex);
        }
    }


    private void setAudioOutputDevice(int deviceIndex)
    {
        FMOD.System coreSystem;
        FMOD.RESULT result = RuntimeManager.StudioSystem.getCoreSystem(out coreSystem);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError($"Failed to get FMOD core system: {result}");
            return;
        }

        result = coreSystem.setDriver(deviceIndex);
        if (result == FMOD.RESULT.OK)
        {
            Debug.Log($"Audio output switched to device index: {deviceIndex}");
        }
        else
        {
            Debug.LogError($"Failed to switch audio output to device index: {deviceIndex}, FMOD Error: {result}");
        }
    }


    private IEnumerator SetDropdownValue(int index)
    {
        yield return new WaitForEndOfFrame(); // Ensure UI is fully initialized before changing value
        audioDeviceDropdown.value = index;
        OnAudioDeviceSelected(index);
    }

}
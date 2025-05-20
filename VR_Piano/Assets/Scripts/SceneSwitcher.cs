using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject startMenu; //start menu object
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private string songSelectSceneName = "SongSelectV2";

    // XR Hand Inputs for pinch gestures
    [SerializeField] private InputActionReference leftHandSelectAction;
    [SerializeField] private InputActionReference rightHandSelectAction;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private float selectionRadius = 0.05f;

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
        if (startMenu == null)
        {
            Debug.LogError("Start Menu object is not assigned!");
            return;
        }

        startMenu.SetActive(true);

        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
        else
            Debug.LogError("Play Button not assigned!");

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        else
            Debug.LogError("Quit Button not assigned!");
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
        Button[] buttons = { playButton, quitButton };

        foreach (Button button in buttons)
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

    public void StartGame()
    {
        Debug.Log("Loading Song Select Scene...");
        SceneManager.LoadScene(songSelectSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}

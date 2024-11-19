using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SwitchScenes : MonoBehaviour
{
    [SerializeField] private SceneAsset playScene; // Assign a scene in the Inspector
    private string playSceneName;

    [SerializeField] private InputActionReference leftHandAction;  // Reference to the left hand action
    [SerializeField] private InputActionReference rightHandAction; // Reference to the right hand action

    private void Awake()
    {
        // Extract the scene name from the SceneAsset
        if (playScene != null)
        {
            playSceneName = playScene.name;
        }
    }

    private void OnEnable()
    {
        // Enable the input actions
        leftHandAction.action.Enable();
        rightHandAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions
        leftHandAction.action.Disable();
        rightHandAction.action.Disable();
    }

    private void Update()
    {
        // Check if either action is triggered
        if (IsButtonPressed(leftHandAction) || IsButtonPressed(rightHandAction))
        {
            SwitchToPlayScene();
        }
    }

    private bool IsButtonPressed(InputActionReference actionReference)
    {
        // Check if the action is performed
        return actionReference.action != null && actionReference.action.triggered;
    }

    public void SwitchToPlayScene()
    {
        if (!string.IsNullOrEmpty(playSceneName))
        {
            SceneManager.LoadScene(playSceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Make sure a scene is assigned in the Inspector.");
        }
    }
}

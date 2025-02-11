using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class sampleSceneSceneSwitcher : MonoBehaviour
{
    //[SerializeField] private SceneAsset sampleScene; // Assign a scene in the Inspector
    private string sampleScene = "SampleScene";
    private string sampleSceneName;

    [SerializeField] private InputActionReference leftHandAction;  // Reference to the left hand action
    [SerializeField] private InputActionReference rightHandAction; // Reference to the right hand action

    private void Awake()
    {
        // Extract the scene name from the SceneAsset
        if (sampleScene != null)
        {
            sampleSceneName = sampleScene;
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
            SwitchTosampleScene();
        }
    }

    private bool IsButtonPressed(InputActionReference actionReference)
    {
        // Check if the action is performed
        return actionReference.action != null && actionReference.action.triggered;
    }

    public void SwitchTosampleScene()
    {
        if (!string.IsNullOrEmpty(sampleSceneName))
        {
            SceneManager.LoadScene(sampleSceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Make sure a scene is assigned in the Inspector.");
        }
    }
}
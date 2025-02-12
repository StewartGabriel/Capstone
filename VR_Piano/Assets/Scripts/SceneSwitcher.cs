using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class SwitchScenes : MonoBehaviour
{
    //[SerializeField] private SceneAsset songSelect; // Assign a scene in the Inspector
    private string songSelect = "SongSelectV2";
    private string songSelectName;

    [SerializeField] private InputActionReference leftHandAction;  // Reference to the left hand action
    [SerializeField] private InputActionReference rightHandAction; // Reference to the right hand action

    private void Awake()
    {
        // Extract the scene name from the SceneAsset
        if (songSelect != null)
        {
            songSelectName = songSelect;
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
            SwitchTosongSelect();
        }
    }

    private bool IsButtonPressed(InputActionReference actionReference)
    {
        // Check if the action is performed
        return actionReference.action != null && actionReference.action.triggered;
    }

    public void SwitchTosongSelect()
    {
        if (!string.IsNullOrEmpty(songSelectName))
        {
            SceneManager.LoadScene(songSelectName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. Make sure a scene is assigned in the Inspector.");
        }
    }
}

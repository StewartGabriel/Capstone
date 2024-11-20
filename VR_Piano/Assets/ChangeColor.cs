using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [Tooltip("Array of Materials to cycle through")]
    public Material[] Materials;

    [Tooltip("Mesh Renderer to target")]
    public Renderer TargetRenderer;

    // use to cycle through our Materials
    private int _currentIndex = 0;

    private TestSound testSound; // Reference to TestSound script
    private void Awake()
    {
        testSound = GetComponent<TestSound>(); // Get the TestSound component
     }

    public void OnChange()
    {
        Debug.Log("Change Color");
        _currentIndex++;

        if (_currentIndex >= Materials.Length) _currentIndex = 0;

        TargetRenderer.material = Materials[_currentIndex];

        // Call the TestSound method
        if (testSound != null)
        {
            testSound.OnChange();
        }
    }
}
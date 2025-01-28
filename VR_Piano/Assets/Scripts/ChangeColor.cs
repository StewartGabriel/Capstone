using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    [Tooltip("Array of Materials to cycle through")]
    public Material[] Materials;
    public int numberOfKeys;

    [Tooltip("Mesh Renderer to target")]
    public Renderer TargetRenderer;

    // use to cycle through our Materials
    private int _currentIndex = 0;

    public void OnChange()
    {
        //Debug.Log("Change Color");
        _currentIndex++;

        if (_currentIndex >= Materials.Length) _currentIndex = 0;

        TargetRenderer.material = Materials[_currentIndex];


    }
}
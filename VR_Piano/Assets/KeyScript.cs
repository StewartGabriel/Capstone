using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Renderer thisKeysRenderer;
    public Material[] Materials;
    
    private bool isactive;
    // Start is called before the first frame update
    void Start()
    {
        thisKeysRenderer.material = Materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void KeyDown()
    {
        Debug.Log("KeyDown called");
        thisKeysRenderer.material = Materials[1];
    }
    public void KeyUp()
    {
        Debug.Log("KeyUp called");
        thisKeysRenderer.material = Materials[0];
    }
}

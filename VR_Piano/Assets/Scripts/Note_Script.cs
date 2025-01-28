using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float growRate; // Rate at which the object stretches
    public float stretchmulti; // Rate at which the object stretches
    public bool on  = true;
    void Start()
    {
    }

    void Update()
    {
        float growthAmount = growRate * Time.deltaTime;
        // Adjust position to compensate for scaling (move backward by half the growth to keep rear edge fixed)
        transform.Translate(Vector3.forward * growthAmount, Space.Self);
        
        if(on){
            
            // Scale the parent along the z-axis
            transform.localScale += new Vector3(0, 0, growthAmount * stretchmulti);
        }
   }
}

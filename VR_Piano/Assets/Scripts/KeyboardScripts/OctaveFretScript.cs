using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaveFret : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void shapefret(float width,float depth)
    {
        Vector3 newscale = transform.localScale;
        newscale.x = width;
        newscale.z = depth;
        newscale.y = width;
        
        transform.localScale = newscale; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoHandle : MonoBehaviour
{

    // Start is called before the first frame update
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void sethandleposition()
    {
        Vector3 position = new Vector3 (PlayerPrefs.GetFloat(gameObject.name + "_Pos_X"),
        PlayerPrefs.GetFloat(gameObject.name + "_Pos_Y"),
        PlayerPrefs.GetFloat(gameObject.name + "_Pos_z"));
        transform.position = position;
    }
}

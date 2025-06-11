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
    public void sethandleposition(float reference)
    {
        Vector3 position = new Vector3 (PlayerPrefs.GetFloat(gameObject.name + "_Pos_X", reference + Random.Range(1f,2f)),
        PlayerPrefs.GetFloat(gameObject.name + "_Pos_Y", reference + Random.Range(1f,2f)),
        PlayerPrefs.GetFloat(gameObject.name + "_Pos_z", 0));
        transform.position = position;
    }
}

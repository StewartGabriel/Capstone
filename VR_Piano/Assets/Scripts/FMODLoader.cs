using FMODUnity;
using UnityEngine;

public class FMODBankLoader : MonoBehaviour
{
    void Start()
    {
        RuntimeManager.LoadBank("Master", true);
        RuntimeManager.LoadBank("Piano", true);
        Debug.Log("Please work! Banks loaded!");
    }
}

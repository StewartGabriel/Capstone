using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    PluginInit pluginInit;
    int last_count;

    // Start is called before the first frame update
    void Start()
    {
        pluginInit = GameObject.Find("Plugin").GetComponent<PluginInit>();
        InitializePlugIn("com.gabriel.midi.PlugInInstance"); // com.gabriel.miditest.PlugInInstance
        if (_pluginInstance != null)
        {
            Debug.Log("Instance Created");
            _pluginInstance.Call("createMidiManager");
            if (_pluginInstance.Get<AndroidJavaObject>("midiManager") != null) // Manager created
            {
                Debug.Log("MIDI Manager Created Somehow");
            }
        }
        last_count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (last_count == 0 && _pluginInstance.Call<int>("getDeviceAmount") != last_count) // Devices array is updated
        {
            Debug.Log("Updating devices connected");
            last_count = 1;
            Debug.Log("Attempting to create port");
            _pluginInstance.Call("createPort"); // createPort is a void method now
                                                // _pluginInstance.Call("Toast", _pluginInstance.Call<string>("createPort"));
        }
    }

    private void InitializePlugIn(string pluginName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);
        if (_pluginInstance == null)
        {
            Debug.Log("Plugin Instance Error");
        }
        _pluginInstance.CallStatic("receiveUnityActivity", unityActivity);

    }

    public void ReceiveMIDI(string msg)
    {
        PlayerInput playerInput = GameObject.Find("Cube").GetComponent<PlayerInput>();
        playerInput.OnChange();
        Debug.Log(msg);
    }

    //TABLET DEBUG METHODS
    // public void Add()
    // {
    //     if (_pluginInstance != null)
    //     {
    //         var result = _pluginInstance.Call<int>("Add", 5, 6);
    //         Debug.Log("Add result from Unity: " + result);
    //     }
    //     else
    //     {
    //         Debug.Log("Add Fail");
    //     }
    // }

    // public void Toast()
    // {
    //     if (_pluginInstance != null)
    //     {
    //         _pluginInstance.Call("Toast", "Hi! from Unity");
    //     }
    //     else
    //     {
    //         Debug.Log("Toast Fail");
    //     }
    // }

    /*public void CreateDevice()
    {

    }

    */

}

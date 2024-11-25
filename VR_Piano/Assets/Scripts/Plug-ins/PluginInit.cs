using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    PluginInit pluginInit;
    PlayerInput playerInput;
    int last_size;

    // Start is called before the first frame update
    void Start()
    {
        pluginInit = GameObject.Find("Cube").GetComponent<PluginInit>();
        playerInput = GameObject.Find("Cube").GetComponent<PlayerInput>();
        InitializePlugIn("com.gabriel.midi.PlugInInstance"); //com.gabriel.miditest.PlugInInstance
        last_size = 0;
        if (_pluginInstance != null)
        {
            Debug.Log("Instance Created");
            _pluginInstance.Call("CreateMidiManager");
            if (_pluginInstance.Get<AndroidJavaObject>("midiManager") != null)
            {
                Debug.Log("MIDI Manager Created Somehow");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Devices Connected: " + _pluginInstance.Call<int>("getDeviceAmount"));
        if (_pluginInstance.Call<int>("getDeviceAmount") != last_size) //Changes the cube's color if a device is connected or disconnected
        {
            last_size = _pluginInstance.Call<int>("getDeviceAmount");
            playerInput.OnChange();
            //Create a reciever
            //_pluginInstance.Call("createReciever");
        }
    }

    void InitializePlugIn(string pluginName) //Initialize the plug-in
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); //Represents the User as a UnityPlayer
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity"); //Represents the User's activity in the app, the one thing they should be interacting with
        _pluginInstance = new AndroidJavaObject(pluginName); //Initiallizes the plug-in 
        if (_pluginInstance == null)
        {
            Debug.Log("Plugin Instance Error");
        }
        _pluginInstance.CallStatic("receiveUnityActivity", unityActivity); // Calls the plug-in
    }




    // TABLET DEBUG METHODS
    // public void Add() //Test Script to add numbers and log them
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

    // public void Toast() //Test Script to show a message to the screen
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

}

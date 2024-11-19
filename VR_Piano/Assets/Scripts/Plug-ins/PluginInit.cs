using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    PluginInit pluginInit;

    // Start is called before the first frame update
    void Start()
    {
        pluginInit = GameObject.Find("Canvas").GetComponent<PluginInit>();
        InitializePlugIn("com.gabriel.midi.PlugInInstance"); //com.gabriel.miditest.PlugInInstance
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

    }

    void InitializePlugIn(string pluginName)
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

    public void Add()
    {
        if (_pluginInstance != null)
        {
            var result = _pluginInstance.Call<int>("Add", 5, 6);
            Debug.Log("Add result from Unity: " + result);
        }
        else
        {
            Debug.Log("Add Fail");
        }
    }

    public void Toast()
    {
        if (_pluginInstance != null)
        {
            _pluginInstance.Call("Toast", "Hi! from Unity");
        }
        else
        {
            Debug.Log("Toast Fail");
        }
    }

    /*public void CreateDevice()
    {

    }

    */

}
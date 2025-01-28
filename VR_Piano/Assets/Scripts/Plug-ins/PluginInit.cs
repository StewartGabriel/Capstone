using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject _pluginInstance;

    PluginInit pluginInit;
    private int last_count;

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

    private void ReceiveMIDI(string msg) //receives the note information from the plug-in
    {
        string[] callback = msg.Split(" ");
        NoteCallback noteCallback = GameObject.Find("Cube").GetComponent<NoteCallback>();
        noteCallback.InterpretMidi(int.Parse(callback[1]), int.Parse(callback[3]));
        Debug.Log(callback[1] + " " + callback[3] + " " + msg);
    }

}

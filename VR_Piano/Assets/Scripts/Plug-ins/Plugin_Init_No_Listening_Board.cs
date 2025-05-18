using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plugin_Init_No_Listening_Board : MonoBehaviour
{
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject _pluginInstance;

    private PluginInit pluginInit;
    private bool searchingForDevices = false;
    private bool firstInitialize = true;

    public event Action<int, int> OnMidiInput;

    void Awake()
    {
        if (GameObject.FindObjectsOfType<Plugin_Init_No_Listening_Board>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        pluginInit = GameObject.Find("Plugin").GetComponent<PluginInit>();
        InitializePlugIn("com.gabriel.midi.PlugInInstance");
        if (_pluginInstance != null)
        {
            Debug.Log("Instance Created");
            _pluginInstance.Call("createMidiManager");
            if (_pluginInstance.Get<AndroidJavaObject>("midiManager") != null)
            {
                DisconnectDevices();
                Debug.Log("MIDI Manager Created");
            }
        }
        else
        {
            Debug.Log("No midi devices found, reset scene to initialize");
        }
    }

    void Update()
    {
        if (_pluginInstance != null)
        {
            int deviceAmount = _pluginInstance.Call<int>("getDeviceAmount");

            if (firstInitialize && deviceAmount != 0)
            {
                firstInitialize = false;
                searchingForDevices = false;
                Debug.Log("No longer searching for devices");
                _pluginInstance.Call("createPort");
            }
            else if (searchingForDevices && deviceAmount != 0)
            {
                searchingForDevices = false;
                Debug.Log("No longer searching for devices");
                _pluginInstance.Call("createPort");
            }
            else if (!searchingForDevices && deviceAmount == 0)
            {
                Debug.Log("No devices connected, starting search");
                searchingForDevices = true;
                DisconnectDevices();
            }
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

    private void ReceiveMIDI(string msg)
    {
        string[] callback = msg.Split(" ");
        if (callback.Length < 4) return;

        int note = int.Parse(callback[1]);
        int velocity = int.Parse(callback[3]);

        Debug.Log($"MIDI Input Received - Note: {note}, Velocity: {velocity}");

        OnMidiInput?.Invoke(note, velocity);
    }

    public void DisconnectDevices()
    {
        Debug.Log("Trying to disconnect a device: " + _pluginInstance.Call<int>("disconnectDevices"));
    }
}

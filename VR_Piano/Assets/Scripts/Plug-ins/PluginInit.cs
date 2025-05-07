using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginInit : MonoBehaviour
{

    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject _pluginInstance;

    private PluginInit pluginInit;
    private bool searchingForDevices = false;
    private bool firstInitialize = true;

    // Awake is called before Start, which is called before the first frame update
    void Awake()
    {
        if (GameObject.FindObjectsOfType<PluginInit>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

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
                DisconnectDevices(); //Ensure no devices are connected
                Debug.Log("MIDI Manager Created");
            }
        }
        else
        {
            Debug.Log("No midi devices found, reset scene to initialize");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_pluginInstance != null)
        {
            if (firstInitialize && _pluginInstance.Call<int>("getDeviceAmount") != 0)
            {
                firstInitialize = false;
                searchingForDevices = false;
                Debug.Log("No longer searching for devices");
                Debug.Log("Attempting to create port");
                _pluginInstance.Call("createPort");
            }

            else if (searchingForDevices)
            {
                if (_pluginInstance.Call<int>("getDeviceAmount") != 0) // Devices array is updated
                {
                    searchingForDevices = false;
                    Debug.Log("No longer searching for devices");
                    Debug.Log("Attempting to create port");
                    _pluginInstance.Call("createPort");
                }
            }
            else if (!searchingForDevices && _pluginInstance.Call<int>("getDeviceAmount") == 0) // no devices currently connected
            {
                Debug.Log("No devices connected, starting search");
                searchingForDevices = true;
                DisconnectDevices(); //Ensure no devices are connected
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

    private void ReceiveMIDI(string msg) //receives the note information from the plug-in
    {
        string[] callback = msg.Split(" ");
        ListeningBoard noteCallback = GameObject.Find("Board Listening").GetComponent<ListeningBoard>();
        noteCallback.InterpretMidi(int.Parse(callback[1]), int.Parse(callback[3]),true);
        Debug.Log(callback[1] + " " + callback[3] + " " + msg);
    }

    public void DisconnectDevices()
    {
        Debug.Log("Trying to disconnect a device in from the Unity application: " + _pluginInstance.Call<int>("disconnectDevices")); //1 for devices closed, 0 for no devices
    }

}

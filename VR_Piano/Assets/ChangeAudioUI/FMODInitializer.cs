using UnityEngine;
using FMODUnity;

public class OculusFMODInitializer : MonoBehaviour
{
    public OculusFMODCallbackHandler oculusFMODCallbackHandler;

    void Start()
    {
        if (oculusFMODCallbackHandler != null)
        {
            FMOD.Studio.System studioSystem = RuntimeManager.StudioSystem;
            oculusFMODCallbackHandler.PreInitialize(studioSystem, (result, message) => // calls the SelectAudioDevices.cs script
            {
                Debug.Log($"FMOD result: {result}, Message: {message}");
            });
        }
        else
        {
            Debug.LogError("OculusFMODCallbackHandler is not assigned.");
        }
    }
}

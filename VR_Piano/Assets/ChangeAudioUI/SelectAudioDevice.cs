using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Oculus FMOD Callback Handler")]
public class OculusFMODCallbackHandler : FMODUnity.PlatformCallbackHandler
{
    private Dictionary<string, System.Guid> audioDrivers = new Dictionary<string, System.Guid>();
    public override void PreInitialize(FMOD.Studio.System studioSystem, Action<FMOD.RESULT, string> reportResult) //Documentation: https://www.fmod.com/docs/2.03/api/core-api-system.html#system_getdriverinfo
    {
        FMOD.System coreSystem;
        FMOD.RESULT result = studioSystem.getCoreSystem(out coreSystem);
        //reportResult(result, "studioSystem.getCoreSystem");

        int driverCount = 0;
        result = coreSystem.getNumDrivers(out driverCount);
        //reportResult(result, $"coreSystem.getNumDrivers {driverCount}");
        for (int i = 0; i < driverCount; i++)
        {
            int rate;
            int channels;
            string name;
            System.Guid guid;
            FMOD.SPEAKERMODE mode;
            Debug.Log(i);
            result = coreSystem.getDriverInfo(i, out name, 256, out guid, out rate, out mode, out channels);
            //reportResult(result, $"coreSystem.getDriverInfo: rate = {rate}, channels = {channels}, guid = {guid}, mode = {mode}, name = {name}");
            audioDrivers[name] = guid;
        }
        printDrivers();
    }

    private void printDrivers()
    {
        foreach (var kvp in audioDrivers)
        {
            Debug.Log($"pringting kvp: {kvp.Key}, {kvp.Value}");
        }
    }

    public Dictionary<string, System.Guid> getAudioDrivers()
    { //This should work for accessing the drivers after its been initialiized
        return audioDrivers;
    }
}
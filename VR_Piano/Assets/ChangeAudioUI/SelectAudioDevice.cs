using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Oculus FMOD Callback Handler")]
public class OculusFMODCallbackHandler : FMODUnity.PlatformCallbackHandler
{
    private Dictionary<string, int> audioDrivers = new Dictionary<string, int>();
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
            audioDrivers[name] = i;
        }
        printDrivers(); //Debugging function to check if things are in the hashmap properly
    }

    private void printDrivers()
    {
        foreach (var kvp in audioDrivers)
        {
            Debug.Log($"pringting kvp: {kvp.Key}, {kvp.Value}");
        }
    }

    public Dictionary<string, int> getAudioDrivers() //Not sure if this will need to be moved to somewhere else, not sure how things are called from the UI
    { //This should work for accessing the drivers after its been initialiized
        return audioDrivers;
    }
}
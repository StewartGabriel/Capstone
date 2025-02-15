using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Oculus FMOD Callback Handler")]
public class OculusFMODCallbackHandler : FMODUnity.PlatformCallbackHandler
{
    public override void PreInitialize(FMOD.Studio.System studioSystem, Action<FMOD.RESULT, string> reportResult) //Documentation: https://www.fmod.com/docs/2.03/api/core-api-system.html#system_getdriverinfo
    {
        FMOD.System coreSystem;
        FMOD.RESULT result = studioSystem.getCoreSystem(out coreSystem);
        reportResult(result, "studioSystem.getCoreSystem");

        int driverCount = 0;
        result = coreSystem.getNumDrivers(out driverCount);
        reportResult(result, $"coreSystem.getNumDrivers {driverCount}");

        for (int i = 0; i < driverCount; i++)
        {
            int rate;
            int channels;
            string name;
            System.Guid guid;
            FMOD.SPEAKERMODE mode;

            result = coreSystem.getDriverInfo(i, out name, 256, out guid, out rate, out mode, out channels);
            reportResult(result, $"coreSystem.getDriverInfo: rate = {rate}, channels = {channels}, guid = {guid}, mode = {mode}, name = {name}");

            //TODO
            //Figure out logic to set audio output device to the meta quest
            //Probably something like this, but I don't know what the MetaQuestId would be

            // if (guid.ToString() == MetaQuestId)
            // {
            //     result = coreSystem.setDriver(i);
            //     reportResult(result, "coreSystem.setDriver");

            //     break;
            // }
        }
    }
}
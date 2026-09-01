
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace RadioPlus
{
    [HarmonyPatch(typeof(RadioSignalManager), "CheckStation")]
    public static class UnlockPatch
    {
        static void Postfix(RadioSignalManager __instance, int index)
        {
            if (!Plugin.UnlockCustom.Value) return;

            var radio = AccessTools.Field(typeof(RadioSignalManager), "radio")
                .GetValue(__instance) as sRadioSystem;
            if (radio == null || index >= radio.channels.Count) return;

            RadioChannel channel = radio.channels[index];
            if (channel.externalTracks == null || channel.externalTracks.Count == 0) return;

            channel.signal = 1f;
        }
    }
}
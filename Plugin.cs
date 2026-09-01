using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;

namespace RadioPlus
{
    [BepInPlugin("com.figrindan.radioplus", "Radio Plus", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> Shuffle;
        internal static ConfigEntry<bool> UnlockCustom;
        private void Awake()
        {
            Log = Logger;
            Shuffle = Config.Bind("Radio", "Shuffle", true, "Shuffle the custom radio playlist.");
            UnlockCustom = Config.Bind("Radio", "UnlockCustomStation", true,
                "Give the custom radio station (88.1) full signal from the start, without activating the radio tower. Note, driving next to towers that haven't yet been activated will interrupt your listening :)");
            new Harmony("com.figrindan.radioplus").PatchAll();
            Logger.LogInfo("Radio Plus loaded");
        }
    }
}
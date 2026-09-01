using HarmonyLib;
using UnityEngine;

namespace RadioPlus
{
    [HarmonyPatch(typeof(RadioChannel), nameof(RadioChannel.GetRandomizedClone))]
    public static class ShufflePatch
    {
        static readonly System.Random rng = new System.Random();

        static void Postfix(AudioClip[] __result)
        {
            if (!Plugin.Shuffle.Value || __result == null || __result.Length < 2) return;

            for (int i = __result.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (__result[i], __result[j]) = (__result[j], __result[i]);
            }
        }
    }
}
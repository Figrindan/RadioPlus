using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace RadioPlus
{
    [HarmonyPatch(typeof(RadioChannel), nameof(RadioChannel.GetRandomizedClone))]
    public static class ShufflePatch
    {
        static readonly System.Random rng = new System.Random();
        static readonly Dictionary<RadioChannel, string[]> storedOrder =
            new Dictionary<RadioChannel, string[]>();

        static void Postfix(RadioChannel __instance, AudioClip[] __result)
        {
            if (!Plugin.Shuffle.Value || __result == null || __result.Length < 2) return;
            if (__instance == null) return;

            if (storedOrder.TryGetValue(__instance, out string[] stored) &&
                MatchesStored(__result, stored))
            {
                ApplyStoredOrder(__result, stored);
                return;
            }

            for (int i = __result.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (__result[i], __result[j]) = (__result[j], __result[i]);
            }

            storedOrder[__instance] = __result.Select(c => c.name).ToArray();
        }

        static bool MatchesStored(AudioClip[] result, string[] stored)
        {
            if (stored == null || stored.Length != result.Length) return false;

            var names = new HashSet<string>(stored);
            for (int i = 0; i < result.Length; i++)
            {
                if (!names.Contains(result[i].name)) return false;
            }
            return true;
        }

        static void ApplyStoredOrder(AudioClip[] result, string[] stored)
        {
            var position = new Dictionary<string, int>();
            for (int i = 0; i < stored.Length; i++)
            {
                if (!position.ContainsKey(stored[i])) position[stored[i]] = i;
            }

            System.Array.Sort(result, (a, b) =>
            {
                int ai = position.TryGetValue(a.name, out int x) ? x : int.MaxValue;
                int bi = position.TryGetValue(b.name, out int y) ? y : int.MaxValue;
                return ai.CompareTo(bi);
            });
        }
    }
}
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace RadioPlus
{
    [HarmonyPatch(typeof(sRadioSystem), "GetStartTimeAndClip")]
    public static class StartTimePatch
    {
        static readonly ConditionalWeakTable<AudioClip[], object> lengthCache =
            new ConditionalWeakTable<AudioClip[], object>();

        static bool Prefix(RadioChannel channel, ref AudioClip clip, ref float __result)
        {
            if (channel == null || channel.queue == null || channel.queue.Length == 0)
            {
                clip = null;
                __result = -1f;
                return false;
            }

            AudioClip[] queue = channel.queue;
            float total;

            if (lengthCache.TryGetValue(queue, out object cached))
            {
                total = (float)cached;
            }
            else
            {
                total = 0f;
                for (int i = 0; i < queue.Length; i++)
                {
                    total += queue[i].length;
                }
                lengthCache.Add(queue, total);
            }

            if (total <= 0f)
            {
                clip = null;
                __result = -1f;
                return false;
            }

            float t = Time.unscaledTime % total;

            for (int i = 0; i < queue.Length; i++)
            {
                if (t <= queue[i].length)
                {
                    clip = queue[i];
                    __result = t;
                    return false;
                }
                t -= queue[i].length;
            }

            clip = null;
            __result = -1f;
            return false;
        }
    }
}
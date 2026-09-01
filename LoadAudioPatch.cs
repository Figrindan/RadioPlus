using System.Collections;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


namespace RadioPlus
{
    [HarmonyPatch(typeof(AudioLoader), "LoadAudio")]
    public static class LoadAudioPatch
    {
        static bool Prefix(AudioLoader __instance, string path, ref IEnumerator __result)
        {
            __result = Replacement(__instance, path);
            return false;
        }
        static readonly HashSet<string> Known = new HashSet<string>
        {
            ".mp3", ".wav", ".ogg", ".flac", ".aiff", ".aif", ".m4a", ".opus", ".mod", ".it", ".s3m", ".xm"
        };

        static readonly HashSet<string> Skip = new HashSet<string>
        {
            ".meta", ".txt", ".ini", ".json", ".jpg", ".jpeg", ".png", ".gif", ".pdf",
            ".zip", ".rar", ".7z", ".exe", ".dll", ".lnk", ".db", ".url", ".cue", ".log", ".m3u", ".m3u8"
        };
        static IEnumerator Replacement(AudioLoader loader, string path)
        {
            string fileName = Path.GetFileName(path);
            string ext = Path.GetExtension(path).ToLowerInvariant();

            if (ext.Length == 0 || Skip.Contains(ext))
            {
            Plugin.Log.LogInfo($"Ignoring {fileName}");
            yield break;
            }

                if (!Known.Contains(ext))
                {
                Plugin.Log.LogInfo($"Trying {fileName} (unrecognized extension)");
                }
            

            using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, GuessType(ext)))
            {
                ((DownloadHandlerAudioClip)req.downloadHandler).streamAudio = true;
                yield return req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Plugin.Log.LogWarning($"Skipping {fileName}: {req.error}");
                    yield break;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);

                if (clip == null)
                {
                    Plugin.Log.LogWarning($"Skipping {fileName}: no clip");
                    yield break;
                }

                clip.name = fileName;

                
                loader.tracks.Add(clip);

                Plugin.Log.LogInfo($"Loaded {fileName} — {clip.length:F2}s, {clip.samples} samples, {clip.frequency}Hz, {clip.channels}ch, state={clip.loadState}");
            }
        }

        static AudioType GuessType(string ext)
        {
            
            if (ext == ".mp3") return AudioType.MPEG;
            if (ext == ".ogg") return AudioType.OGGVORBIS;
            if (ext == ".wav") return AudioType.WAV;
            return AudioType.UNKNOWN;
        }
    }
}
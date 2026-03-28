using UnityEngine;
using System;
using System.IO;

[Serializable]
public class StoryProgressData
{
    public StoryAct currentAct;
    public int act1MatchIndex;
    public int act3MatchIndex;
    public bool act4Triggered;
    public bool storyComplete;
}

public static class StorySaveSystem
{
    private static readonly string SavePath =
        Path.Combine(Application.persistentDataPath, "story.sav");

    private static readonly string Key = "ks_story";

    public static void Save (StoryManager storyManager)
    {
        StoryProgressData data = new StoryProgressData
        {
            currentAct      = storyManager.GetCurrentAct(),
            act1MatchIndex  = storyManager.GetAct1MatchIndex(),
            act3MatchIndex  = storyManager.GetAct3MatchIndex(),
            act4Triggered   = storyManager.GetAct4Triggered(),
            storyComplete   = storyManager.GetStoryComplete()
        };

        string json = JsonUtility.ToJson(data);
        string encrypted = XOREncrypt(json, Key);
        File.WriteAllText(SavePath, encrypted);
    }

    public static StoryProgressData Load()
    {
        if (!File.Exists(SavePath))
            return null;

        string encrypted = File.ReadAllText(SavePath);
        string json = XOREncrypt(encrypted, Key);
        return JsonUtility.FromJson<StoryProgressData>(json);
    }

    public static void Delete() => File.Delete(SavePath);

    private static string XOREncrypt(string input, string key)
    {
        char[] output = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
            output[i] = (char)(input[i] ^ key[i % key.Length]);
        return new string(output);
    }
}

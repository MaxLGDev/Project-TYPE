using UnityEngine;
using System.IO;
using System.Diagnostics.Tracing;

public class WordLoader : MonoBehaviour
{
    public string[] words;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        string path = Path.Combine(Application.dataPath, "Data/Words/wordList.txt");
        words = File.ReadAllLines(path);
    }
}

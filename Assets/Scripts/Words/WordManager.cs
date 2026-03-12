using UnityEngine;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance { get; private set; }

    [Header("Words List")]
    [SerializeField] private WordLoader wordLoader;
    private string targetWord;

    [SerializeField] private int minLength;
    [SerializeField] private int maxLength;

    public string CurrentWord => targetWord;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("There are 2 word managers active!!!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GetNextWord();
    }

    public void GetNextWord()
    {
        if (wordLoader.words == null || wordLoader.words.Length == 0)
            return;

        string newWord;
        int wordRerollAttempt = 0;

        do
        {
            int index = Random.Range(0, wordLoader.words.Length);
            newWord = wordLoader.words[index];
            wordRerollAttempt++;
            if(wordRerollAttempt > 100)
            {
                Debug.Log($"returning: 100 attempts failed, min:{minLength} max:{maxLength} last word tried:'{newWord}'");
                return;
            }

        }
        while (newWord == targetWord || newWord.Length < minLength || newWord.Length > maxLength);

        targetWord = newWord;
    }

    public void UpdateWordFilter(int min, int max)
    {
        minLength = min;
        maxLength = max;
    }
}

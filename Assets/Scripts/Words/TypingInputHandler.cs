using System.Collections;
using UnityEngine;
public class TypingInputHandler : MonoBehaviour
{
    public static TypingInputHandler Instance { get; private set; }

    public event System.Action OnWordCompleted;

    [SerializeField] private float mistypeLockDuration = 0.2f;

    public string TypedSoFar => typedSoFar;
    private string typedSoFar;
    public bool LastKeyWrong;

    private Coroutine mistypeCO;
    private bool isLocked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("2 input handlers!!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        RegisterTyping();
        CheckCompletion();
    }

    private void RegisterTyping()
    {
        if (isLocked)
            return;

        foreach (char typedChar in Input.inputString)
        {
            if (typedChar == '\b')
            {
                if (typedSoFar.Length > 0)
                    typedSoFar = typedSoFar.Substring(0, typedSoFar.Length - 1);
                LastKeyWrong = false;
            }
            else if (char.IsLetter(typedChar) && typedChar <= 'z' && typedChar >= 'a')
            {
                char lowerCase = char.ToLower(typedChar);
                string current = WordManager.Instance.CurrentWord;
                if (typedSoFar.Length < current.Length && lowerCase == current[typedSoFar.Length])
                {
                    typedSoFar += lowerCase;
                    LastKeyWrong = false;
                }
                else
                {
                    LastKeyWrong = true;
                    if (mistypeCO != null)
                        StopCoroutine(mistypeCO);

                    mistypeCO = StartCoroutine(LockInputPenalty());
                }
            }
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        isLocked = !enabled;
    }

    public bool ConsumeWrongKey()
    {
        if (!LastKeyWrong)
            return false;

        LastKeyWrong = false;
        return true;
    }

    private void CheckCompletion()
    {
        if (string.IsNullOrEmpty(typedSoFar))
            return;

        if (typedSoFar == WordManager.Instance.CurrentWord)
        {
            typedSoFar = "";
            LastKeyWrong = false;
            WordManager.Instance.GetNextWord();
            OnWordCompleted?.Invoke();
            
        }
    }

    public void ResetTyping()
    {
        typedSoFar = "";
    }

    private IEnumerator LockInputPenalty()
    {
        isLocked = true;
        float elapsed = 0f;

        while (elapsed < mistypeLockDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        isLocked = false;
    }
}

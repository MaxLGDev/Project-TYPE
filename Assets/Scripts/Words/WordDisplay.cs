using System.Collections;
using TMPro;
using UnityEngine;

public class WordDisplay : MonoBehaviour
{
    [SerializeField] private WordManager wordManager;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeDuration = 0.3f;

    private Vector3 originalPos;
    private Coroutine shakeCO;
    private bool isWrong;
    private string lastTyped = "";

    private void Start()
    {
        originalPos = displayText.transform.localPosition;
    }

    private void OnEnable()
    {
        TypingInputHandler.Instance.OntypingReset += ResetDisplay;
    }

    private void OnDisable()
    {
        TypingInputHandler.Instance.OntypingReset -= ResetDisplay;
    }

    private void Update()
    {
        string current = wordManager.CurrentWord;
        string typed = TypingInputHandler.Instance.TypedSoFar;
        bool wrong = TypingInputHandler.Instance.ConsumeWrongKey();

        // correct key typed = typed string grew
        if (typed.Length > lastTyped.Length)
        {
            isWrong = false;

            if(shakeCO != null)
                StopCoroutine(shakeCO);
            shakeCO = null;
            displayText.transform.localPosition = originalPos;
        }

        if(typed.Length < lastTyped.Length)
        {
            isWrong = false;
        }

        if (wrong) 
            isWrong = true;

        lastTyped = typed;

        UpdateText(current, typed, isWrong);

        if (wrong)
        {
            if (shakeCO != null) StopCoroutine(shakeCO);
            shakeCO = StartCoroutine(Shake());
        }
    }

    private void UpdateText(string current, string typed, bool wrong)
    {
        int matchLength = Mathf.Min(typed.Length, current.Length);
        string matched = current.Substring(0, matchLength);
        string remaining = current.Substring(matchLength);

        if (wrong && remaining.Length > 0)
        {
            string nextChar = remaining.Substring(0, 1);
            string rest = remaining.Substring(1);
            displayText.text = $"<color=green>{matched}</color><color=red>{nextChar}</color>{rest}";
        }
        else
        {
            displayText.text = $"<color=green>{matched}</color>{remaining}";
        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeAmount, shakeAmount);
            float y = Random.Range(-shakeAmount, shakeAmount);
            displayText.transform.localPosition = originalPos + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        displayText.transform.localPosition = originalPos;
        shakeCO = null;
    }

    public void ResetDisplay()
    {
        lastTyped = "";
        isWrong = false;
        displayText.transform.localPosition = originalPos;
    }
}
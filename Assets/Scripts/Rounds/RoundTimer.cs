using TMPro;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    public event System.Action OnTimerExpired;

    [Header("Round details")]
    [SerializeField] private float roundDuration = 60f;
    private float timeRemaining;
    private bool timerExpired = false;

    private TMP_Text timerText;

    private void Start()
    {
        timerText = GetComponent<TMP_Text>();

        if (timerText == null)
        {
            Debug.LogError("No timer text!");
            return;
        }

        ResetTimer();
    }

    private void Update()
    {
        ReduceTimer();
    }

    private void ReduceTimer()
    {
        if (timerExpired)
            return;

        timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            timerExpired = true;
            OnTimerExpired?.Invoke();
        }
    }

    private void ResetTimer()
    {
        timeRemaining = roundDuration;
        timerExpired = false;
    }
}

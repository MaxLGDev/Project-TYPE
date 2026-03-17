using TMPro;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    public event System.Action OnTimerExpired;

    [Header("Round details")]
    [SerializeField] private float roundDuration = 60f;
    private float timeRemaining;
    private bool timerExpired = false;

    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
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
        if (GameManager.Instance.CurrentGameState != GameState.InMatch || timerExpired)
            return;

        timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            timerExpired = true;
            OnTimerExpired?.Invoke();
        }
    }

    public void ResetTimer()
    {
        timeRemaining = roundDuration;
        timerExpired = false;
    }
}

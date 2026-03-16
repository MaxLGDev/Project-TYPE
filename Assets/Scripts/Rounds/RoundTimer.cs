using TMPro;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    public event System.Action OnTimerExpired;

    [Header("Round details")]
    [SerializeField] private float roundDuration = 60f;
    private float timeRemaining;
    private bool timerExpired = false;
    private bool paused = false;

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
        if (timerExpired || paused)
            return;

        timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            timerExpired = true;
            OnTimerExpired?.Invoke();
        }
    }

    public void PauseTimer(bool isPaused)
    {
        paused = isPaused;
    }

    public void ResetTimer()
    {
        timeRemaining = roundDuration;
        timerExpired = false;
        paused = false;
    }
}

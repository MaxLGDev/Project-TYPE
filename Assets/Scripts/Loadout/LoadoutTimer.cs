using TMPro;
using UnityEngine;

public class LoadoutTimer : MonoBehaviour
{
    public event System.Action OnTimerExpired;

    [SerializeField] private float weaponPhaseDuration = 20f;
    [SerializeField] private float powerPhaseDuration = 15f;
    [SerializeField] private float mapPhaseDuration = 10f;

    [SerializeField] private TMP_Text loadoutTitleText;
    [SerializeField] private TMP_Text timerText;

    private GameState currentState;
    private float timeRemaining;
    private bool timerExpired = false;

    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("No timer text!");
            return;
        }

        ResetTimer();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= HandlePhaseChanged;
    }

    private void Update()
    {
        ReduceTimer();
    }

    private void HandlePhaseChanged(GameState state)
    {
        if(state == GameState.LoadoutWeapons || state == GameState.LoadoutPowers || state == GameState.LoadoutMap)
            ResetTimer();
    }

    private void ReduceTimer()
    {
        if (timerExpired)
            return;

        if (GameManager.Instance.CurrentGameState != GameState.LoadoutWeapons &&
            GameManager.Instance.CurrentGameState != GameState.LoadoutPowers &&
            GameManager.Instance.CurrentGameState != GameState.LoadoutMap)
            return;

        timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
        loadoutTitleText.text = GetPhaseName();
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            OnTimerExpired?.Invoke();
            timerExpired = true;
        }
    }

    private string GetPhaseName()
    {
        return GameManager.Instance.CurrentGameState switch
        {
            GameState.LoadoutWeapons => "WEAPONS",
            GameState.LoadoutPowers => "SPECIALS",
            GameState.LoadoutMap => "MAP",
            _ => ""
        };
    }

    private float GetTimerDuration() => currentState switch
    {
        GameState.LoadoutWeapons => weaponPhaseDuration,
        GameState.LoadoutPowers => powerPhaseDuration,
        GameState.LoadoutMap => mapPhaseDuration,
        _ => 0f
    };

    public void ResetTimer()
    {
        currentState = GameManager.Instance.CurrentGameState;
        timeRemaining = GetTimerDuration();
        timerExpired = false;
    }
}

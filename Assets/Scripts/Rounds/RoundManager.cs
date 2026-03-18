using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public event System.Action<PlayerID> OnRoundEnded;
    public event System.Action<PlayerID> OnMatchEnded;

    [SerializeField] private RoundTimer roundTimer;

    [Header("Player 1 Details")]
    [SerializeField] private PlayerHealth p1Health;
    [SerializeField] private CombatManager p1Combat;
    [SerializeField] private PowerManager p1Power;
    private int p1RoundWins = 0;
    public int P1RoundWins => p1RoundWins;
    public float P1CurrentHP => p1Health.CurrentHP;

    [Header("Player 2 Details")]
    [SerializeField] private PlayerHealth p2Health;
    [SerializeField] private CombatManager p2Combat;
    [SerializeField] private PowerManager p2Power;
    private int p2RoundWins = 0;
    public int P2RoundWins => p2RoundWins;
    public float P2CurrentHP => p2Health.CurrentHP;

    private PlayerID? firstToFire = null;
    private int roundCount = 1;
    public int RoundCount => roundCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("2 round managers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (roundTimer == null)
            Debug.LogError("RoundTimer not assigned!");

        if (p1Health == null)
            Debug.LogError("Player Health 1 not assigned!");

        if (p2Health == null)
            Debug.LogError("Player Health 2 not assigned!");

        firstToFire = null;
    }

    private void OnEnable()
    {
        roundTimer.OnTimerExpired += HandleTimerExpired;

        p1Health.OnDeath += HandleEndRound;
        p1Combat.OnWeaponFired += HandleWeaponFired;

        p2Health.OnDeath += HandleEndRound;
        p2Combat.OnWeaponFired += HandleWeaponFired;
    }

    private void OnDisable()
    {
        roundTimer.OnTimerExpired -= HandleTimerExpired;

        p1Health.OnDeath -= HandleEndRound;
        p1Combat.OnWeaponFired -= HandleWeaponFired;

        p2Health.OnDeath -= HandleEndRound;
        p2Combat.OnWeaponFired -= HandleWeaponFired;
    }

    private void EndRound(PlayerID winner)
    {
        if (GameManager.Instance.CurrentGameState != GameState.InMatch)
            return;

        if (winner == PlayerID.Player1)
            p1RoundWins++;
        else
            p2RoundWins++;

        if (p1RoundWins >= 3 || p2RoundWins >= 3)
            EndMatch(winner);
        else
        {
            OnRoundEnded?.Invoke(winner);
        }
    }

    public void StartNextRound()
    {
        roundCount++;
        firstToFire = null;
        p1Health.ResetHealth();
        p1Power.ResetPower();

        p2Health.ResetHealth();
        p2Power.ResetPower();

        roundTimer.ResetTimer();
        
        TypingInputHandler.Instance.ResetTyping();
    }

    public void ResetMatch()
    {
        roundCount = 1;
        p1RoundWins = 0;
        p2RoundWins = 0;
        StartNextRound();
        GameManager.Instance.SetState(GameState.InMatch);
    }

    private void EndMatch(PlayerID winner)
    {
        Debug.Log("EndMatch called, firing OnMatchEnded");
        if (GameManager.Instance.CurrentGameState == GameState.InMatch)
            OnMatchEnded?.Invoke(winner);
    }

    private void HandleTimerExpired()
    {
        if (p1Health.CurrentHP > p2Health.CurrentHP)
            EndRound(PlayerID.Player1);
        else if (p2Health.CurrentHP > p1Health.CurrentHP)
            EndRound(PlayerID.Player2);
        else
        {
            if (firstToFire != null)
                EndRound(firstToFire.Value);
            else
                EndRound(PlayerID.Player1); // fallback, shouldn't happen
        }
    }

    private void HandleEndRound()
    {
        if (p1Health.CurrentHP <= 0)
            EndRound(PlayerID.Player2);
        else if (p2Health.CurrentHP <= 0)
            EndRound(PlayerID.Player1);
    }

    private void HandleWeaponFired(PlayerID player)
    {
        if (firstToFire == null)
            firstToFire = player;
    }
}

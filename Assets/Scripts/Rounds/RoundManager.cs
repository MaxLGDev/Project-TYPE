using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private RoundTimer roundTimer;

    [Header("Player 1 Details")]
    [SerializeField] private PlayerHealth p1Health;
    [SerializeField] private CombatManager p1Combat;

    [Header("Player 2 Details")]
    [SerializeField] private PlayerHealth p2Health;
    [SerializeField] private CombatManager p2Combat;

    private PlayerID? firstToFire = null;
    private bool roundEnded = false;

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

    private void EndRound(string winner)
    {
        if (roundEnded)
            return;

        roundEnded = true;
        Debug.Log($"{winner} won!");
    }

    private void HandleTimerExpired()
    {
        if (p1Health.CurrentHP > p2Health.CurrentHP)
            EndRound("Player 1 wins!");
        else if (p2Health.CurrentHP > p1Health.CurrentHP)
            EndRound("Player 2 wins!");
        else
        {
            if (firstToFire != null)
                EndRound($"{firstToFire} wins!");
            else
                EndRound("Draw!");
        }
    }

    private void HandleEndRound()
    {
        if (p1Health.CurrentHP <= 0)
            EndRound("Player 2 wins!");
        else if (p2Health.CurrentHP <= 0)
            EndRound("Player 1 wins!");
    }

    private void HandleWeaponFired(PlayerID player)
    {
        if (firstToFire == null)
            firstToFire = player;
    }
}

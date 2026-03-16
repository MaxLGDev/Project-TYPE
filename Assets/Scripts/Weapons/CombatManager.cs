using System.Collections;
using UnityEngine;

public enum PlayerID
{
    Player1,
    Player2
}

public class CombatManager : MonoBehaviour
{
    public event System.Action<PlayerID> OnWeaponFired;

    [SerializeField] private PlayerHealth targetHealth;
    private LoadoutManager playerLoadout;

    [SerializeField] private PlayerID playerID;

    private bool effectOnCooldown = false;

    private Coroutine effectCdCO;

    private void Start()
    {
        playerLoadout = GetComponent<LoadoutManager>();
    }

    private void OnEnable()
    {
        if (TypingInputHandler.Instance != null)
            TypingInputHandler.Instance.OnWordCompleted += HandleWordCompleted;
    }

    private void OnDisable()
    {
        if (TypingInputHandler.Instance != null)
            TypingInputHandler.Instance.OnWordCompleted -= HandleWordCompleted;
    }

    private void HandleWordCompleted()
    {
        WeaponData weapon = playerLoadout.ActiveWeapon;
        targetHealth.TakeDamage(weapon.damagePerShot);
        OnWeaponFired?.Invoke(playerID);

        if (!effectOnCooldown)
        {

            switch (playerLoadout.ActiveWeapon.specialEffect)
            {
                case SpecialEffect.Flicker:
                    EffectManager.Instance.ApplyFlicker();
                    ApplyEffectCD(weapon.specialEffectCD);
                    break;
                default:
                    break;
            }
        }
    }

    private void ApplyEffectCD(float cd)
    {
        if (effectCdCO != null)
            StopCoroutine(effectCdCO);

        effectCdCO = StartCoroutine(EffectCooldownCO(cd));
    }

    private IEnumerator EffectCooldownCO(float cdTimer)
    {
        effectOnCooldown = true;
        float elapsed = 0f;

        while (elapsed < cdTimer)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        effectOnCooldown = false;
    }
}

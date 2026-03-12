using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth targetHealth;
    [SerializeField] private LoadoutManager targetLoadout;

    private bool effectOnCooldown = false;

    private Coroutine effectCdCO;

    private void Start()
    {
        TypingInputHandler.Instance.OnWordCompleted += HandleWordCompleted;
    }

    private void HandleWordCompleted()
    {
        WeaponData weapon = targetLoadout.ActiveWeapon;
        targetHealth.TakeDamage(weapon.damagePerShot);

        if (!effectOnCooldown)
        {

            switch (targetLoadout.ActiveWeapon.specialEffect)
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

    private void OnDestroy()
    {
        TypingInputHandler.Instance.OnWordCompleted -= HandleWordCompleted;
    }
}

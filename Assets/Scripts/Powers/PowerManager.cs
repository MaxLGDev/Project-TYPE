using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private PowerData activePower;
    [SerializeField] private PlayerID playerID;
    [SerializeField] private PowerSlotUI powerSlotUI;

    private PlayerHealth playerHealth;

    [SerializeField] private bool isAI = false;

    private bool onCooldown;
    private bool usedThisRound = false;

    private Coroutine cooldownCO;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerID == PlayerID.Player1 && GameManager.Instance.P1SelectedPower != null)
            activePower = GameManager.Instance.P1SelectedPower;

        if (activePower != null)
            powerSlotUI.UpdateState(false, false, 0f);
    }

    private void Update()
    {
        CheckInGameState();
    }

    public void ResetPower()
    {
        Debug.Log($"ResetPower called, cooldownCO null: {cooldownCO == null}");

        if (activePower == null)
            return;

        if (cooldownCO != null)
            StopCoroutine(cooldownCO);

        usedThisRound = false;
        onCooldown = false;
        powerSlotUI.UpdateState(onCooldown, usedThisRound, activePower.CooldownDuration);
    }

    private void CheckInGameState()
    {
        if (!isAI)
        {
            if (GameManager.Instance.CurrentGameState == GameState.InMatch && Keyboard.current.spaceKey.wasPressedThisFrame)
                TryActiveSpecial();
        }
    }

    private void TryActiveSpecial()
    {
        if (activePower == null)
            return;

        if (activePower.OneTimeUse && usedThisRound)
            return;

        if (onCooldown)
            return;

        ActivateSpecial();
    }

    private void ActivateSpecial()
    {
        switch (activePower.PowerEffectType)
        {
            case PowerEffectType.Heal:
                GetPlayerHealth().Heal(activePower.PowerValue);
                break;
            default:
                break;
        }

        if (activePower.OneTimeUse)
            usedThisRound = true;
        else
            cooldownCO = StartCoroutine(StartCooldownCO(activePower.CooldownDuration));

        powerSlotUI.UpdateState(onCooldown, usedThisRound, activePower.CooldownDuration);
    }

    private IEnumerator StartCooldownCO(float cdTimer)
    {
        onCooldown = true;
        float elapsed = 0f;

        while (elapsed < cdTimer)
        {
            if(GameManager.Instance.CurrentGameState == GameState.InMatch)
                elapsed += Time.deltaTime;
            powerSlotUI.UpdateState(onCooldown, usedThisRound, cdTimer - elapsed);
            yield return null;
        }

        onCooldown = false;
        powerSlotUI.UpdateState(onCooldown, usedThisRound, 0f);
    }


    private PlayerHealth GetPlayerHealth()
    {
        return playerHealth;
    }
}

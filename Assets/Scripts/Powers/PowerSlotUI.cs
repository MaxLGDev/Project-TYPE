using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerSlotUI : MonoBehaviour
{
    [SerializeField] private PowerData powerData;

    [SerializeField] private TMP_Text cdText;

    [SerializeField] private Image slotImage;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = Color.gray;

    private void Start()
    {
        if (powerData == null)
            return;

        cdText.text = powerData.CooldownDuration.ToString();

        slotImage.color = activeColor;
    }

    public void UpdateState(bool onCooldown, bool usedThisRound, float cooldownRemaining)
    {
        if (usedThisRound)
        {
            slotImage.color = inactiveColor;
            cdText.text = "USED";
        }

        if(onCooldown)
        {
            slotImage.color = inactiveColor;
            cdText.text = $"<color=yellow>{Mathf.CeilToInt(cooldownRemaining)}</color>";
        }
        else
        {
            slotImage.color = activeColor;
            cdText.text = "READY";
        }
    }
}

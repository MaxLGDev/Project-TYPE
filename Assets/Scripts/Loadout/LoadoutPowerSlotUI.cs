using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutPowerSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text powerName;
    [SerializeField] private Button slotButton;

    private PowerData currentPower;

    private void Start()
    {
        if (slotButton != null)
            slotButton.interactable = false;

        ClearSlot();
    }

    public void SetPower(PowerData data)
    {
        currentPower = data;
        powerName.text = data.PowerName;
        if (slotButton != null)
            slotButton.interactable = true;
    }

    public void SetLocked(bool locked)
    {
        if (slotButton != null)
        {
            if (!locked && currentPower != null)
                slotButton.interactable = true;
            else
                slotButton.interactable = false;
        }
    }

    public void ClearSlot()
    {
        currentPower = null;
        powerName.text = string.Empty;

        if (slotButton != null)
            slotButton.interactable = false;
    }

    public void OnSlotClicked()
    {
        if (currentPower == null)
            return;

        LoadoutScreenManager.Instance.ClearPowerSlot();
    }
}

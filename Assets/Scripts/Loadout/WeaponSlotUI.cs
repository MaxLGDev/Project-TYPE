using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private Button slotButton;

    private WeaponData currentWeapon;

    private void Start()
    {
        if(slotButton != null)
            slotButton.interactable = false;

        ClearSlot();
    }

    public void SetWeapon(WeaponData data)
    {
        currentWeapon = data;
        weaponName.text = data.WeaponName;
        if (slotButton != null)
            slotButton.interactable = true;
    }

    public void SetLocked(bool locked)
    {
        if (slotButton != null)
        {
            if (!locked && currentWeapon != null)
                slotButton.interactable = true;
            else
                slotButton.interactable = false;
        }
    }

    public void ClearSlot()
    {
        currentWeapon = null;
        weaponName.text = string.Empty;

        if (slotButton != null)
            slotButton.interactable = false;
    }

    public void OnSlotClicked()
    {
        if (currentWeapon == null)
            return;

        LoadoutScreenManager.Instance.ClearSlot(slotIndex);
    }
}

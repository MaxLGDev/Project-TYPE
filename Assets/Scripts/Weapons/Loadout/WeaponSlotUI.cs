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
        ClearSlot();
        slotButton.interactable = false;
    }

    public void SetWeapon(WeaponData data)
    {
        currentWeapon = data;
        weaponName.text = data.WeaponName;
        slotButton.interactable = true;
    }

    public void ClearSlot()
    {
        currentWeapon = null;
        weaponName.text = string.Empty;
        slotButton.interactable = false;
    }

    public void OnSlotClicked()
    {
        if (currentWeapon == null)
            return;

        LoadoutScreenManager.Instance.ClearSlot(slotIndex);
    }
}

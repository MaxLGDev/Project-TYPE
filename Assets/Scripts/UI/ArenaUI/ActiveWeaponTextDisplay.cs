using TMPro;
using UnityEngine;

public class ActiveWeaponTextDisplay : MonoBehaviour
{
    [SerializeField] private LoadoutManager playerLoadout;

    private TMP_Text weaponNameText;

    private void Awake()
    {
        weaponNameText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        playerLoadout.OnSlotSwitch += HandleWeaponDisplay;
    }

    private void OnDisable()
    {
        playerLoadout.OnSlotSwitch -= HandleWeaponDisplay;
    }

    private void HandleWeaponDisplay(int activeSlot)
    {
        weaponNameText.text = playerLoadout.ActiveWeapon.WeaponName;
    }
}

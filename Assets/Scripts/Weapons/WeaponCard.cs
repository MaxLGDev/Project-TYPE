using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCard : MonoBehaviour
{
    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private Button cardButton;

    public WeaponData WeaponData { get; private set; }

    public void Setup(WeaponData data)
    {
        WeaponData = data;
        weaponName.text = data.WeaponName;
    }

    public void SetLocked(bool locked)
    {
        cardButton.interactable = !locked;
    }

    public void OnCardClicked()
    {
        LoadoutScreenManager.Instance.OnWeaponSelected(WeaponData);
    }
}

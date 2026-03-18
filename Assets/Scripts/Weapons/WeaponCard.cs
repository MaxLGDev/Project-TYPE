using TMPro;
using UnityEngine;

public class WeaponCard : MonoBehaviour
{
    [SerializeField] private TMP_Text weaponName;

    public WeaponData WeaponData { get; private set; }

    public void Setup(WeaponData data)
    {
        WeaponData = data;
        weaponName.text = data.WeaponName;
    }

    public void OnCardClicked()
    {
        LoadoutScreenManager.Instance.OnWeaponSelected(WeaponData);
    }
}

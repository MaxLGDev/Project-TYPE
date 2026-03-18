using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutScreenManager : MonoBehaviour
{
    public static LoadoutScreenManager Instance;

    [SerializeField] private Button lockButton;
    [SerializeField] private TMP_Text readyText;

    //[SerializeField] private WeaponSlo
    private WeaponData[] p1Weapons = new WeaponData[3];
    [SerializeField] private WeaponSlotUI[] p1Slots;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("2 loadotu screen managers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        UpdateLockButton();
    }

    public void OnWeaponSelected(WeaponData data)
    {
        for(int i = 0; i < p1Weapons.Length; i++)
        {
            if (p1Weapons[i] == null)
            {
                p1Weapons[i] = data;
                p1Slots[i].SetWeapon(data);
                break;
            }
        }

        UpdateLockButton();
    }

    public void ClearSlot(int index)
    {
        p1Weapons[index] = null;
        p1Slots[index].ClearSlot();

        UpdateLockButton();
    }

    private void UpdateLockButton()
    {
        lockButton.interactable = CanLock();

        if (lockButton.interactable)
            readyText.text = "READY";
        else
            readyText.text = "NOT READY";
    }

    private bool CanLock()
    {
        for(int i = 0; i < p1Weapons.Length;i++)
        {
            if (p1Weapons[i] == null)
                return false;
        }

        return true;
    }
}

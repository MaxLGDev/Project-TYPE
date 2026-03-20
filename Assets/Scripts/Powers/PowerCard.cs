using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text powerName;
    [SerializeField] private Button cardButton;

    public PowerData PowersData { get; private set; }

    public void Setup(PowerData data)
    {
        PowersData = data;
        powerName.text = data.PowerName;
    }

    public void SetLocked(bool locked)
    {
        cardButton.interactable = !locked;
    }

    public void OnCardClicked()
    {
        LoadoutScreenManager.Instance.OnPowerSelected(PowersData);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class LoadoutManager : MonoBehaviour
{
    public event System.Action<int> OnSlotSwitch;

    [SerializeField] private WordManager wordManager;

    public WeaponData[] equippedWeapons = new WeaponData[3];
    private int activeSlot = 0;

    [SerializeField] private bool isAI = false;

    public WeaponData ActiveWeapon => equippedWeapons[activeSlot];

    private void Start()
    {
        AssignPlayerLoadout();
        
        if (equippedWeapons[0] != null)
        {
            wordManager.UpdateWordFilter(equippedWeapons[0].minWordLength, equippedWeapons[0].maxWordLength);
            wordManager.GetNextWord(); // first word called here, filter already set
        }

        OnSlotSwitch?.Invoke(activeSlot);
    }

    private void AssignPlayerLoadout()
    {
        if (isAI)
        {
            if (GameManager.Instance.P2SelectedWeapons != null)
            {
                for (int i = 0; i < equippedWeapons.Length; i++)
                    equippedWeapons[i] = GameManager.Instance.P2SelectedWeapons[i];
            }
        }
        else
        {
            if (GameManager.Instance.P1SelectedWeapons != null)
            {
                for (int i = 0; i < equippedWeapons.Length; i++)
                    equippedWeapons[i] = GameManager.Instance.P1SelectedWeapons[i];
            }
        }
    }

    private void Update()
    {
        if (isAI)
            return;

        if (GameManager.Instance.CurrentGameState != GameState.InMatch)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SwitchToSlot(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SwitchToSlot(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SwitchToSlot(2);
        if (Keyboard.current.tabKey.wasPressedThisFrame) CycleSlot();
    }

    private void SwitchToSlot(int slot)
    {
        if (activeSlot == slot)
            return;

        activeSlot = slot;

        if (wordManager != null)
        {
            wordManager.UpdateWordFilter(equippedWeapons[activeSlot].minWordLength, equippedWeapons[activeSlot].maxWordLength);
            wordManager.GetNextWord();
        }

        if (TypingInputHandler.Instance != null)
            TypingInputHandler.Instance.ResetTyping();

        OnSlotSwitch?.Invoke(activeSlot);
    }

    private void CycleSlot() => SwitchToSlot((activeSlot + 1) % 3);
}

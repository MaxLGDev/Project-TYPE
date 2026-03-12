using UnityEngine;
using UnityEngine.InputSystem;

public class LoadoutManager : MonoBehaviour
{
    public WeaponData[] equippedWeapons = new WeaponData[3];
    private int activeSlot = 0;

    public WeaponData ActiveWeapon => equippedWeapons[activeSlot];

    private void Update()
    {
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

        if (WordManager.Instance != null)
        {
            WordManager.Instance.UpdateWordFilter(equippedWeapons[activeSlot].minWordLength, equippedWeapons[activeSlot].maxWordLength);
            WordManager.Instance.GetNextWord();
        }

        if(TypingInputHandler.Instance != null)
            TypingInputHandler.Instance.ResetTyping();
    }

    private void CycleSlot() => SwitchToSlot((activeSlot + 1) % 3);
}

using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotInGameUI : MonoBehaviour
{
    [SerializeField] private LoadoutManager playerLoadout;

    private CanvasGroup canvasGroup;
    [SerializeField] private Image iconBorder;
    [SerializeField] private Image iconImage;

    [SerializeField] private int slotIndex;

    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;
    private Vector3 originalScale;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        playerLoadout.OnSlotSwitch += HandleSlotSwitch;
    }

    private void OnDisable()
    {
        playerLoadout.OnSlotSwitch -= HandleSlotSwitch;
    }

    private void HandleSlotSwitch(int activeSlot)
    {
        SetActive(slotIndex == activeSlot);
    }

    private void SetActive(bool active)
    {
        iconBorder.enabled = active;
        canvasGroup.alpha = active ? 1f : 0.5f;

        if (active)
            transform.localScale = originalScale * maxScale;
        else
            transform.localScale = originalScale * minScale;
    }
}

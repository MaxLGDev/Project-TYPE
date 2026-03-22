using UnityEngine;
using UnityEngine.UI;

public class MapItem : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void SetActive(bool active)
    {
        transform.localScale = active ? Vector3.one * 1.2f : Vector3.one * 0.8f;
        canvasGroup.alpha = active ? 1f : 0.6f;
    }
}
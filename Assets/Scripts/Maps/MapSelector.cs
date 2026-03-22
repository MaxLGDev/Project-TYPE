using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PreviewState
{
    public Vector2 position;
    public float scale;
    public float alpha;
}

public class MapSelector : MonoBehaviour
{
    [SerializeField] private MapDatabase mapDatabase;
    [SerializeField] private TMP_Text mapNameText;

    [Header("Map Previews")]
    [SerializeField] private Image leftPreview;   // map preview on the left, inactive
    [SerializeField] private Image centerPreview; // active map preview
    [SerializeField] private Image rightPreview;  // map preview on the right, inactive

    [Header("Preview States")]
    [SerializeField] private PreviewState leftExitState;
    [SerializeField] private PreviewState leftState;
    [SerializeField] private PreviewState centerState;
    [SerializeField] private PreviewState rightState;
    [SerializeField] private PreviewState rightExitState;
    [SerializeField] private float lerpDuration;

    [Header("Rect and CanvasGroup References")]
    [SerializeField] private RectTransform[] previewsRects;
    [SerializeField] private CanvasGroup[] canvasGroups;

    private int currentIndex = 0;

    private void Start()
    {
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[0]);
        UpdateDisplay();
    }

    public void NextMap()
    {
        currentIndex = (currentIndex + 1) % mapDatabase.maps.Count;
        UpdateSprites();
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[currentIndex]);

        StartCoroutine(NextMapIncomingCo());
        StartCoroutine(AnimateToStateCo(previewsRects[1], canvasGroups[1], leftState, lerpDuration));
        StartCoroutine(AnimateToStateCo(previewsRects[2], canvasGroups[2], centerState, lerpDuration));
    }
    public void PreviousMap()
    {
        currentIndex = (currentIndex - 1 + mapDatabase.maps.Count) % mapDatabase.maps.Count;
        UpdateSprites();
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[currentIndex]);
        
        StartCoroutine(AnimateToStateCo(previewsRects[0], canvasGroups[0], leftState, lerpDuration));
        StartCoroutine(AnimateToStateCo(previewsRects[1], canvasGroups[1], centerState, lerpDuration));
        StartCoroutine(PreviousMapIncomingCo());
    }

    private void UpdateDisplay()
    {
        SnapToState(previewsRects[0], canvasGroups[0], leftState);
        SnapToState(previewsRects[1], canvasGroups[1], centerState);
        SnapToState(previewsRects[2], canvasGroups[2], rightState);
    }

    private void UpdateSprites()
    {
        leftPreview.sprite = mapDatabase.maps[(currentIndex - 1 + mapDatabase.maps.Count) % mapDatabase.maps.Count].previewSprite;
        centerPreview.sprite = mapDatabase.maps[currentIndex].previewSprite;
        rightPreview.sprite = mapDatabase.maps[(currentIndex + 1) % mapDatabase.maps.Count].previewSprite;
        mapNameText.text = mapDatabase.maps[currentIndex].mapName;
    }

    private void SnapToState(RectTransform rect, CanvasGroup canvas, PreviewState state)
    {
        rect.localPosition = state.position;
        rect.localScale = Vector3.one * state.scale;
        canvas.alpha = state.alpha;
    }

    private IEnumerator AnimateToStateCo(RectTransform rect, CanvasGroup canvas, PreviewState state, float duration)
    {
        Vector2 startPos = rect.localPosition;
        float startScale = rect.localScale.x;
        float startAlpha = canvas.alpha;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            rect.localPosition = Vector2.Lerp(startPos, state.position, elapsed / duration);
            rect.localScale = Vector3.one * Mathf.Lerp(startScale, state.scale, elapsed / duration);
            canvas.alpha = Mathf.Lerp(startAlpha, state.alpha, elapsed / duration);

            yield return null;
        }

        rect.localPosition = state.position;
        rect.localScale = Vector3.one * state.scale;
        canvas.alpha = state.alpha;
    }

    private IEnumerator NextMapIncomingCo()
    {
        yield return StartCoroutine(AnimateToStateCo(previewsRects[0], canvasGroups[0], leftExitState, lerpDuration));

        leftPreview.sprite = mapDatabase.maps[(currentIndex + 1) % mapDatabase.maps.Count].previewSprite;

        SnapToState(previewsRects[0], canvasGroups[0], rightExitState);

        yield return StartCoroutine(AnimateToStateCo(previewsRects[0], canvasGroups[0], rightState, lerpDuration));
    }

    private IEnumerator PreviousMapIncomingCo()
    {
        yield return StartCoroutine(AnimateToStateCo(previewsRects[2], canvasGroups[2], rightExitState, lerpDuration));

        rightPreview.sprite = mapDatabase.maps[(currentIndex - 1 + mapDatabase.maps.Count) % mapDatabase.maps.Count].previewSprite;

        SnapToState(previewsRects[2], canvasGroups[2], leftExitState);

        yield return StartCoroutine(AnimateToStateCo(previewsRects[2], canvasGroups[2], leftState, lerpDuration));
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

public class FilmStripSelector : MonoBehaviour
{
    [SerializeField] private MapDatabase mapDatabase;
    [SerializeField] private TMP_Text mapNameText;
    [SerializeField] private RectTransform contentStrip;
    [SerializeField] private float itemWidth = 260f;
    [SerializeField] private float lerpDuration = 0.25f;
    [SerializeField] private MapItem[] mapItems;

    private int currentIndex = 0;
    private bool isAnimating = false;

    private Vector2 startPos = new Vector2(200f, 0f);

    private void Start()
    {
        currentIndex = 0;
        contentStrip.anchoredPosition = startPos;
        UpdateUI();
    }

    public void NextMap()
    {
        if (isAnimating || currentIndex >= mapDatabase.maps.Count - 1) return;
        currentIndex++;
        StartCoroutine(SlideCo());
    }

    public void PreviousMap()
    {
        if (isAnimating || currentIndex <= 0) return;
        currentIndex--;
        StartCoroutine(SlideCo());
    }

    private IEnumerator SlideCo()
    {
        isAnimating = true;
        Vector2 start = contentStrip.anchoredPosition;
        Vector2 target = new Vector2(startPos.x + currentIndex * -itemWidth, 0);
        float elapsed = 0f;
        while (elapsed < lerpDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            contentStrip.anchoredPosition = Vector2.Lerp(start, target, Mathf.SmoothStep(0, 1, elapsed / lerpDuration));
            yield return null;
        }
        contentStrip.anchoredPosition = target;
        UpdateUI();
        isAnimating = false;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < mapItems.Length; i++)
            mapItems[i].SetActive(i == currentIndex);

        mapNameText.text = mapDatabase.maps[currentIndex].mapName;
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[currentIndex]);
    }
}
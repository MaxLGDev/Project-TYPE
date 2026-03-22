using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    [SerializeField] private MapDatabase mapDatabase;
    [SerializeField] private TMP_Text mapNameText;

    [Header("Map Previews")]
    [SerializeField] private Image leftPreview;   // map preview on the left, inactive
    [SerializeField] private Image centerPreview; // active map preview
    [SerializeField] private Image rightPreview;  // map preview on the right, inactive

    private int currentIndex = 0;

    private void Start()
    {
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[0]);
        UpdateDisplay();
    }

    public void NextMap()
    {
        currentIndex = (currentIndex + 1) % mapDatabase.maps.Count;
        UpdateDisplay();
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[currentIndex]);
    }
    public void PreviousMap()
    {
        currentIndex = (currentIndex - 1 + mapDatabase.maps.Count) % mapDatabase.maps.Count;
        UpdateDisplay();
        GameManager.Instance.SetSelectedMap(mapDatabase.maps[currentIndex]);
    }

    private void UpdateDisplay()
    {
        leftPreview.sprite = mapDatabase.maps[(currentIndex - 1 + mapDatabase.maps.Count) % mapDatabase.maps.Count].previewSprite;
        centerPreview.sprite = mapDatabase.maps[currentIndex].previewSprite;
        rightPreview.sprite = mapDatabase.maps[(currentIndex + 1) % mapDatabase.maps.Count].previewSprite;
        mapNameText.text = mapDatabase.maps[currentIndex].mapName;
    }
}

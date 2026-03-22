using UnityEngine;
using UnityEngine.UI;

public class ArenaBackgroundDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Image>().sprite = GameManager.Instance.SelectedMap.backgroundSprite;
    }
}

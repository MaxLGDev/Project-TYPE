using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject offlineMatchObject;

    [SerializeField] private GameObject difficulties;

    private void Start()
    {
        offlineMatchObject.SetActive(true);
        difficulties.SetActive(false);
    }

    public void ShowDifficulties()
    {
        offlineMatchObject.SetActive(false);
        difficulties.SetActive(true);
    }
}

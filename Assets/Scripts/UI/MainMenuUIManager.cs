using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
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

    public void OnDifficultyClicked(int index) => GameManager.Instance.PlayVSIA(index);
}

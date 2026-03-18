using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        RoundManager.Instance.ResetMatch();
    }

    public void LoadLoadoutScene()
    {
        GameManager.Instance.SetState(GameState.LoadoutWeapons);
        SceneManager.LoadScene("LoadoutSelectionScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        RoundManager.Instance.ResetMatch();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

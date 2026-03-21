using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private float transitionDuration = 2f;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReloadScene()
    {
        RoundManager.Instance.ResetMatch();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCO(sceneName, transitionDuration));
    }

    private IEnumerator LoadSceneCO(string sceneName, float duration)
    {
        GameManager.Instance.EndingSceneTransition.SetActive(true);
        yield return new WaitForSeconds(duration);
        GameManager.Instance.EndingSceneTransition.SetActive(false);

        SceneManager.LoadScene(sceneName);

        
        GameManager.Instance.StartingSceneTransition.SetActive(true);
        yield return new WaitForSeconds(duration);
        GameManager.Instance.StartingSceneTransition.SetActive(false);
    }
}

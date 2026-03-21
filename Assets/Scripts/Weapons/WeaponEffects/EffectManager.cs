using System.Collections;
using TMPro;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] private TMP_Text p1DisplayText;
    [SerializeField] private TMP_Text p2DisplayText;

    [Header("Flicker details")]
    [SerializeField] private float flickerTotalDuration = 1.5f;
    [SerializeField] private float singleFlickerDuration = 0.2f;

    private Coroutine FlickerCO;

    private void Awake()
    {
        if(Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ApplyFlicker(PlayerID target)
    {
        TMP_Text targetText = target == PlayerID.Player1 ? p1DisplayText : p2DisplayText; 
        if(FlickerCO != null)
            StopCoroutine(FlickerCO);

        FlickerCO = StartCoroutine(FlickerRoutine(flickerTotalDuration, targetText));
    }

    private IEnumerator FlickerRoutine(float duration, TMP_Text target)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.enabled = !target.enabled;
            yield return new WaitForSeconds(singleFlickerDuration);
            elapsed += singleFlickerDuration;
        }

        target.enabled = true;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "ArenaScene")
        {
            // find the display texts by tag or name
            p1DisplayText = GameObject.FindWithTag("WordDisplay/P1")?.GetComponent<TMP_Text>();
            p2DisplayText = GameObject.FindWithTag("WordDisplay/P2")?.GetComponent<TMP_Text>();
        }
    }
}

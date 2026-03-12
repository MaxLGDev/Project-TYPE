using System.Collections;
using TMPro;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] private TMP_Text wordDisplayText;

    [Header("Flicker details")]
    [SerializeField] private float flickerTotalDuration = 1.5f;
    [SerializeField] private float singleFlickerDuration = 0.2f;

    private Coroutine FlickerCO;

    private void Awake()
    {
        if(Instance != null &&  Instance != this)
        {
            Debug.Log("2 effect managers!!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyFlicker()
    {
        if(FlickerCO != null)
            StopCoroutine(FlickerCO);

        FlickerCO = StartCoroutine(FlickerRoutine(flickerTotalDuration));
    }

    private IEnumerator FlickerRoutine(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            wordDisplayText.enabled = !wordDisplayText.enabled;
            yield return new WaitForSeconds(singleFlickerDuration);
            elapsed += singleFlickerDuration;
        }

        wordDisplayText.enabled = true;
    }
}

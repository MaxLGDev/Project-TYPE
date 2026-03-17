using System.Collections;
using UnityEngine;

public enum AIDifficulty
{
    Easy,
    Medium,
    Hard,
    Insane,
    Nightmare,
    Demonic,
    Impossible
}

public class AIController : MonoBehaviour
{
    public event System.Action OnAIWordCompleted;

    [Header("References")]
    [SerializeField] private WordManager wordManager;
    [SerializeField] private LoadoutManager loadoutManager;
    [SerializeField] private CombatManager combatManager;

    [Header("Difficulty")]
    [SerializeField] private AIDifficulty difficulty;
    private float wpmSpeed;

    private string aiTypedSoFar;
    private Coroutine typeWordCO;

    public string AITypedSoFar => aiTypedSoFar;

    private void Start()
    {
        wpmSpeed = GetWPM();
        wordManager.GetNextWord();

        aiTypedSoFar = "";

        SetAIPaused(true);
    }

    private float GetCharDelay()
    {
        float baseDelay = 60f / (wpmSpeed * 5f);
        return Random.Range(baseDelay * 0.97f, baseDelay * 1.03f); // Small speed variation for small "alive" feeling
    }

    private float GetWPM() => difficulty switch
    {
        AIDifficulty.Easy => 25f,
        AIDifficulty.Medium => 50f,
        AIDifficulty.Hard => 75f,
        AIDifficulty.Insane => 100f,
        AIDifficulty.Nightmare => 125f,
        AIDifficulty.Demonic => 150f,
        AIDifficulty.Impossible => 175f,
        _ => 25f
    };

    private IEnumerator TypeCurrentWordCO()
    {
        while (true)
        {
            string word = wordManager.CurrentWord;
            aiTypedSoFar = "";

            foreach (char c in word)
            {
                yield return new WaitForSeconds(GetCharDelay());
                aiTypedSoFar += c;
            }
            OnAIWordCompleted?.Invoke();
            wordManager.GetNextWord();
        }
        
    }

    public void SetAIPaused(bool active)
    {
        if (active)
            typeWordCO = StartCoroutine(TypeCurrentWordCO());
        else if(typeWordCO != null)
            StopCoroutine(typeWordCO);
    }
}

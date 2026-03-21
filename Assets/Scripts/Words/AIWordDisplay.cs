using TMPro;
using UnityEngine;

public class AIWordDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WordManager wordManager;
    [SerializeField] private AIController controller;
    [SerializeField] private TMP_Text displayText;

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState != GameState.InMatch)
        {
            displayText.enabled = false;
            return;
        }
        displayText.enabled = true;

        string current = wordManager.CurrentWord;
        string typed = controller.AITypedSoFar;

        int matchLength = Mathf.Min(typed.Length, current.Length);
        string matched = current.Substring(0, matchLength);
        string remaining = current.Substring(matchLength);
        displayText.text = $"<color=red>{matched}</color>{remaining}";
    }
}

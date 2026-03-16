using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Round Transition")]
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TMP_Text roundWinnerText;
    [SerializeField] private TMP_Text roundP1HPText;
    [SerializeField] private TMP_Text roundP2HPText;
    [SerializeField] private float delayDuration = 3f;

    [Header("Match End")]
    [SerializeField] private GameObject matchEndedPanel;
    [SerializeField] private TMP_Text matchWinnerText;
    [SerializeField] private TMP_Text matchP1HPText;
    [SerializeField] private TMP_Text matchP2HPText;

    [SerializeField] private TMP_Text[] roundScoreTexts;

    private Coroutine transitionCO;

    private void OnEnable()
    {
        RoundManager.Instance.OnRoundEnded += HandleRoundTransition;
        RoundManager.Instance.OnMatchEnded += HandleMatchEnd;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundEnded -= HandleRoundTransition;
        RoundManager.Instance.OnMatchEnded -= HandleMatchEnd;
    }

    private void HandleRoundTransition(PlayerID winner)
    {
        UpdateRoundScore();
        UpdateRoundWinner(winner);
        ShowRoundHP();

        transitionPanel.SetActive(true);
        TypingInputHandler.Instance.SetInputEnabled(false);

        if (transitionCO != null)
            StopCoroutine(transitionCO);

        transitionCO = StartCoroutine(DelayBeforeNextRound());
    }

    private void HandleMatchEnd(PlayerID winner)
    {
        UpdateRoundScore();
        UpdateMatchWinner(winner);
        ShowMatchHP();

        matchEndedPanel.SetActive(true);
        TypingInputHandler.Instance.SetInputEnabled(false);
    }

    [ContextMenu("Reset Match")]
    private void ResetMatch()
    {
        RoundManager.Instance.ResetMatch();
        UpdateRoundScore();
    }

    private IEnumerator DelayBeforeNextRound()
    {
        yield return new WaitForSeconds(delayDuration);
        transitionPanel.SetActive(false);

        if (!RoundManager.Instance.IsMatchEnded)
        {
            TypingInputHandler.Instance.SetInputEnabled(true);
            RoundManager.Instance.StartNextRound();
        }
    }

    private void UpdateRoundScore()
    {
        string score = $"{RoundManager.Instance.P1RoundWins} - {RoundManager.Instance.P2RoundWins}";
        foreach (TMP_Text text in roundScoreTexts)
            text.text = score;
    }

    private void UpdateRoundWinner(PlayerID winner)
    {
        roundWinnerText.text = $"ROUND {RoundManager.Instance.RoundCount} - {GetPlayerName(winner)} wins!";
    }

    private void UpdateMatchWinner(PlayerID winner)
    {
        matchWinnerText.text = $"{GetPlayerName(winner)} has won!";
    }

    private void ShowRoundHP()
    {
        roundP1HPText.text = $"P1: {RoundManager.Instance.P1CurrentHP}";
        roundP2HPText.text = $"P2: {RoundManager.Instance.P2CurrentHP}";
    }

    private void ShowMatchHP()
    {
        matchP1HPText.text = $"P1: {RoundManager.Instance.P1CurrentHP}";
        matchP2HPText.text = $"P2: {RoundManager.Instance.P2CurrentHP}";
    }

    private string GetPlayerName(PlayerID id)
    {
        return id == PlayerID.Player1 ? "Player 1" : "Player 2";
    }
}

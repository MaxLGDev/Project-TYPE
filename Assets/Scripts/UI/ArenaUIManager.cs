using System.Collections;
using TMPro;
using UnityEngine;

public class ArenaUIManager : MonoBehaviour
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
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnded += HandleRoundTransition;
            RoundManager.Instance.OnMatchEnded += HandleMatchEnd;
        }
        else
        {
            Debug.LogError("RoundManager not found in UIManager OnEnable!");
            return;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnded -= HandleRoundTransition;
            RoundManager.Instance.OnMatchEnded -= HandleMatchEnd;
        }
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.InMatch)
        {
            transitionPanel.SetActive(false);
            matchEndedPanel.SetActive(false);
            UpdateRoundScore();
        }
    }

    private void HandleRoundTransition(PlayerID winner)
    {
        UpdateRoundScore();
        UpdateRoundWinner(winner);
        ShowRoundHP();

        transitionPanel.SetActive(true);

        if (transitionCO != null)
            StopCoroutine(transitionCO);

        transitionCO = StartCoroutine(DelayBeforeNextRound());
    }

    private void HandleMatchEnd(PlayerID winner)
    {
        Debug.Log("UIManager HandleMatchEnd called");
        UpdateRoundScore();
        UpdateMatchWinner(winner);
        ShowMatchHP();

        matchEndedPanel.SetActive(true);
    }

    private IEnumerator DelayBeforeNextRound()
    {
        Debug.Log("Delay started");
        yield return new WaitForSeconds(delayDuration);
        Debug.Log($"Delay finished, state: {GameManager.Instance.CurrentGameState}");

        if (GameManager.Instance.CurrentGameState != GameState.MatchOver)
        {
            transitionPanel.SetActive(false);
            RoundManager.Instance.StartNextRound();
            GameManager.Instance.SetState(GameState.PreRound);
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

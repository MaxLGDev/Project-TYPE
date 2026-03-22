using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownDisplay : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TMP_Text countdownText;

    private Coroutine countdownCO;

    private void Start()
    {
        ResetCountdown();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += StartCountdown;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= StartCountdown;
    }

    private void StartCountdown(GameState state)
    {
        if (state != GameState.PreRound)
            return;

        countdownPanel.SetActive(true);

        if(countdownCO != null)
            StopCoroutine(countdownCO);

        countdownCO = StartCoroutine(CountdownCo());
    }

    private IEnumerator CountdownCo()
    {
        countdownText.enabled = true;

        for(int i = 3; i >= 1; i--)
        {
            countdownText.text = $"{i.ToString()}...";
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "FIGHT!";
        yield return new WaitForSecondsRealtime(0.5f);

        countdownPanel.SetActive(false);
        GameManager.Instance.SetState(GameState.InMatch);
    }

    public void ResetCountdown()
    {
        countdownPanel.SetActive(true);
        countdownText.text = "";
    }
}

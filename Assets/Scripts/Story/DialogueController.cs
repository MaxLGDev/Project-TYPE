using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Dialogue UI References")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;

    [Header("Environmental Dialogue UI References")]
    [SerializeField] private TMP_Text environmentalText;
    [SerializeField] private GameObject environmentalPanel;

    private bool skipRequested;
    private Coroutine activeSequence;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnPreMatchDialogueRequested += PlaySequence;
        gameManager.OnPostMatchDialogueRequested += PlaySequence;
    }

    private void OnDisable()
    {
        gameManager.OnPreMatchDialogueRequested -= PlaySequence;
        gameManager.OnPostMatchDialogueRequested -= PlaySequence;
    }

    private void Update()
    {
        if(activeSequence != null && Input.anyKeyDown)
            skipRequested = true;
    }

    public void Skip()
    {
        skipRequested = true;
    }

    public void PlaySequence(DialogueLineData[] lines, Action onComplete)
    {
        activeSequence = StartCoroutine(PlaySequenceCo(lines, onComplete));
    }

    private IEnumerator PlaySequenceCo(DialogueLineData[]lines, Action onComplete)
    {
        foreach(DialogueLineData line in lines)
        {
            skipRequested = false;

            if(line.isEnvironmental)
            {
                environmentalPanel.SetActive(true);
                environmentalText.text = line.dialogueText;
            }
            else
            {
                dialoguePanel.SetActive(true);
                speakerNameText.text = line.speakerName;
                dialogueText.text = line.dialogueText;
            }

            if (line.displayDuration > 0)
            {
                float elapsed = 0f;
                yield return new WaitUntil(() =>
                {
                    elapsed += Time.deltaTime;
                    return skipRequested || elapsed >= line.displayDuration;
                });
            }
            else
                yield return new WaitUntil(() => skipRequested);

            dialoguePanel.SetActive(false);
            environmentalPanel.SetActive(false);
        }

        activeSequence = null;
        onComplete?.Invoke();
    }

    public void Interrupt(Action onComplete)
    {
        if (activeSequence != null)
        {
            StopCoroutine(activeSequence);
            activeSequence = null;
        }

        dialoguePanel.SetActive(false);
        environmentalPanel.SetActive(false);
        onComplete?.Invoke();
    }
}

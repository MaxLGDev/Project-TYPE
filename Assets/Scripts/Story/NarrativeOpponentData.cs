using UnityEngine;

[CreateAssetMenu(fileName = "New Opponent", menuName = ("KEYSTRIKE/Opponents"))]
public class NarrativeOpponentData : ScriptableObject
{
    [Header("Opponent Infos")]
    public string opponentName;

    [Header("Story Position")]
    public StoryAct act;
    public int matchIndex;

    [Header("Loadout Infos")]
    public WeaponData[] loadoutSlots;
    public AIDifficulty aiDifficultyLevel;

    [Header("Dialogue")]
    public DialogueLineData[] preMatchLines;
    public DialogueLineData[] postMatchLines;

    //[Header("Dialogue Infos")]
    //public DialogueLine[] preMatchLines;
    //public DialogueLine[] postLossLines;
    //public string[] foreshadowingFlags;

    public bool MatchesCurrentState(StoryAct currentAct, int currentMatchIndex)
    {
        return act == currentAct && matchIndex == currentMatchIndex;
    }
}

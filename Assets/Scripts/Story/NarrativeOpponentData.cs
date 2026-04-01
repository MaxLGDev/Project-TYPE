using UnityEngine;

[CreateAssetMenu(fileName = "New Opponent", menuName = ("KEYSTRIKE/Dialogues/Opponents"))]
public class NarrativeOpponentData : ScriptableObject
{
    [Header("Opponent Infos")]
    public string OpponentName;

    [Header("Story Position")]
    public StoryAct Act;
    public int MatchIndex;

    [Header("Loadout Infos")]
    public WeaponData[] LoadoutSlots;
    public AIDifficulty AiDifficultyLevel;

    [Header("Dialogue")]
    public DialogueLineData[] PreMatchLines;
    public DialogueLineData[] PostMatchLines;
    //public string[] foreshadowingFlags;

    public bool MatchesCurrentState(StoryAct currentAct, int currentMatchIndex)
    {
        return Act == currentAct && MatchIndex == currentMatchIndex;
    }
}

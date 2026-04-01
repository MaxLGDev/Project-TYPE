using UnityEngine;

[CreateAssetMenu(fileName = "New Act Dialogue", menuName = "KEYSTRIKE/Dialogues/Act Dialogue")]
public class StorySequenceData : ScriptableObject
{
    public StoryAct StoryAct;
    public DialogueLineData[] ActLines;
}

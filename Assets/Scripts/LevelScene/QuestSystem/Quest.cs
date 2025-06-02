using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questID; // Save için eþsiz kimlik
    public string questName;
    public string description;
    public QuestObjective[] objectives;

    public Quest prerequisiteQuest;

    public bool IsCompleted()
    {
        foreach (var obj in objectives)
        {
            if (!obj.IsComplete())
                return false;
        }
        return true;
    }

    public bool IsAvailable()
    {
        return prerequisiteQuest == null || QuestManager.Instance.IsQuestCompleted(prerequisiteQuest);
    }
}

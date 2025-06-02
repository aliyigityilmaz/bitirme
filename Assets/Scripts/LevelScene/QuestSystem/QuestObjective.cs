using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public string description;
    public bool isCompleted;

    public abstract void CheckProgress();

    public bool IsComplete()
    {
        return isCompleted;
    }

    public void CompleteObjective()
    {
        isCompleted = true;
    }
}

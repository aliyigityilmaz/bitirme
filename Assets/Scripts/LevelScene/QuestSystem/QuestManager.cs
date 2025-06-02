using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private List<Quest> activeQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
            activeQuests.Add(quest);
    }

    public bool IsQuestCompleted(Quest quest)
    {
        return quest.IsCompleted();
    }

    public void RegisterKill(string tag)
    {
        foreach (var quest in activeQuests)
        {
            foreach (var objective in quest.objectives)
            {
                //if (objective is KillObjective killObj)
                    //killObj.RegisterKill(tag);
            }
        }
    }

    public void RegisterCollect(string itemName)
    {
        foreach (var quest in activeQuests)
        {
            foreach (var objective in quest.objectives)
            {
                //if (objective is CollectObjective collectObj)
                    //collectObj.RegisterCollection(itemName);
            }
        }
    }
}

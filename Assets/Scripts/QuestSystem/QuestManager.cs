using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField]
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuest(Quest quest)
    {
        if (!activeQuests.ContainsKey(quest.questName))
        {
            activeQuests[quest.questName] = quest;
            QuestUIManager.Instance.AddQuestUI(quest); // UI güncelle
        }
    }

    public void CompleteQuest(string questName)
    {
        if (activeQuests.ContainsKey(questName))
        {
            activeQuests[questName].isCompleted = true;
            QuestUIManager.Instance.CompleteQuestUI(questName); // UI'dan kaldýr
        }
    }


    public bool IsQuestCompleted(string questName)
    {
        return activeQuests.ContainsKey(questName) && activeQuests[questName].isCompleted;
    }
}

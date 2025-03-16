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

    // Yeni bir görev ekler (Eðer zaten ekli deðilse)
    public void AddQuest(Quest quest)
    {
        if (quest == null) return;

        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            Debug.Log("New Quest Added: " + quest.questName);
        }
        else
        {
            Debug.Log("Quest already active: " + quest.questName);
        }
    }

    // Görevi tamamlandý olarak iþaretler
    public void CompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);
        if (quest != null)
        {
            quest.isCompleted = true;
            Debug.Log("Quest Completed: " + quest.questName);
        }
    }

    // Görev tamamlandý mý?
    public bool IsQuestCompleted(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);
        return quest != null && quest.isCompleted;
    }
}

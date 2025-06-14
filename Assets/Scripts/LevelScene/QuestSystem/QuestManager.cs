using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private HashSet<string> acquiredQuests = new HashSet<string>();
    private HashSet<string> completedQuests = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçiþlerinde korunur
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintActiveQuests();
        }
    }

    public void AddQuest(Quest quest)
    {
        if (!acquiredQuests.Contains(quest.questID))
        {
            acquiredQuests.Add(quest.questID);
            Debug.Log($"Görev alýndý: {quest.questName} (ID: {quest.questID})");
        }
        else
        {
            Debug.Log($"Zaten sahip olunan görev: {quest.questName} (ID: {quest.questID})");
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (!CanCompleteQuest(quest)) return;

        if (!completedQuests.Contains(quest.questID))
        {
            completedQuests.Add(quest.questID);

            // Eðer item gerekiyorsa, düþür
            if (quest.requiredItem != null && quest.requiredItemCount > 0)
            {
                BackpackManager.Instance.RemoveItem(quest.requiredItem, quest.requiredItemCount);
            }

            Debug.Log($"Görev tamamlandý: {quest.questName} (ID: {quest.questID})");
        }
    }


    public bool CanCompleteQuest(Quest quest)
    {
        if (!HasQuest(quest.questID) || IsQuestCompleted(quest.questID))
            return false;

        if (quest.requiredItem != null)
        {
            int playerItemCount = BackpackManager.Instance.GetItemCount(quest.requiredItem);
            return playerItemCount >= quest.requiredItemCount;
        }

        // Eðer item yoksa, yani sadece konuþarak tamamlanabiliyorsa:
        return true;
    }



    public bool HasQuest(string questID) => acquiredQuests.Contains(questID);

    public bool IsQuestCompleted(string questID) => completedQuests.Contains(questID);

    public void CompleteQuest(string questID)
    {
        if (acquiredQuests.Contains(questID))
            completedQuests.Add(questID);
    }

    public void PrintActiveQuests()
    {
        Debug.Log("=== Aktif Görevler ===");
        if (acquiredQuests.Count == 0)
        {
            Debug.Log("Hiç görev alýnmamýþ.");
            return;
        }

        foreach (var questID in acquiredQuests)
        {
            Debug.Log($"- {questID}");
        }
    }


    public List<string> GetAllAcquiredQuests() => new List<string>(acquiredQuests);
}

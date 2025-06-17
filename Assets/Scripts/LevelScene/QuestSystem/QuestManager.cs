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
            Debug.Log($"G�rev al�nd�: {quest.questName} (ID: {quest.questID})");
            RefreshUI();
        }
        else
        {
            Debug.Log($"Zaten sahip olunan g�rev: {quest.questName} (ID: {quest.questID})");
        }

        
    }

    private void RefreshUI()
    {
        List<Quest> questList = new List<Quest>();

        foreach (string id in acquiredQuests)
        {
            // Sahnedeki t�m quest'leri bul ve ID e�le�eni al
            Quest[] allQuests = Resources.FindObjectsOfTypeAll<Quest>();
            Quest quest = System.Array.Find(allQuests, q => q.questID == id);
            if (quest != null)
                questList.Add(quest);
        }

        QuestUIController.Instance?.RefreshQuestUI(questList);
    }

    public void CompleteQuest(Quest quest)
    {
        if (!CanCompleteQuest(quest)) return;

        if (!completedQuests.Contains(quest.questID))
        {
            completedQuests.Add(quest.questID);

            // E�er item gerekiyorsa, d���r
            if (quest.requiredItem != null && quest.requiredItemCount > 0)
            {
                BackpackManager.Instance.RemoveItem(quest.requiredItem, quest.requiredItemCount);
            }

            Debug.Log($"G�rev tamamland�: {quest.questName} (ID: {quest.questID})");
            RefreshUI();
        }
    }


    public bool CanCompleteQuest(Quest quest)
    {
        if (!HasQuest(quest.questID) || IsQuestCompleted(quest.questID))
            return false;

        // E�er item gerekiyorsa:
        if (quest.requiredItem != null)
        {
            int playerItemCount = BackpackManager.Instance.GetItemCount(quest.requiredItem);
            return playerItemCount >= quest.requiredItemCount;
        }

        // Sadece konu�ma g�reviyse:
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
        Debug.Log("=== Aktif G�revler ===");
        if (acquiredQuests.Count == 0)
        {
            Debug.Log("Hi� g�rev al�nmam��.");
            return;
        }

        foreach (var questID in acquiredQuests)
        {
            Debug.Log($"- {questID}");
        }
    }


    public List<string> GetAllAcquiredQuests() => new List<string>(acquiredQuests);
}

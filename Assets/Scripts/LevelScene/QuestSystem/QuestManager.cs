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
            DontDestroyOnLoad(gameObject); // Sahne ge�i�lerinde korunur
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
        }
        else
        {
            Debug.Log($"Zaten sahip olunan g�rev: {quest.questName} (ID: {quest.questID})");
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (acquiredQuests.Contains(quest.questID) && !completedQuests.Contains(quest.questID))
        {
            completedQuests.Add(quest.questID);
            Debug.Log($"G�rev tamamland�: {quest.questName} (ID: {quest.questID})");
        }
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

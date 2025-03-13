using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    public GameObject questPrefab; // Quest UI prefab�
    public Transform questListContainer; // Vertical Group buraya ba�l� olacak

    private Dictionary<string, GameObject> questUIItems = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuestUI(Quest quest)
    {
        if (questUIItems.ContainsKey(quest.questName)) return; // Zaten ekliyse tekrar ekleme

        GameObject questItem = Instantiate(questPrefab, questListContainer);
        questItem.GetComponent<TextMeshProUGUI>().text = quest.questName;
        questItem.transform.Find("QuestDesc").GetComponent<TextMeshProUGUI>().text=quest.questDescription;

        questUIItems[quest.questName] = questItem;
    }

    public void CompleteQuestUI(string questName)
    {
        if (questUIItems.ContainsKey(questName))
        {
            Destroy(questUIItems[questName]); // UI'dan kald�r
            questUIItems.Remove(questName);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    public GameObject questUIPanel;
    public Transform questContainer;
    public GameObject questPrefab;

    private Dictionary<string, GameObject> activeQuestUI = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddQuestUI(Quest quest)
    {
        if (activeQuestUI.ContainsKey(quest.questName)) return;

        GameObject questUI = Instantiate(questPrefab, questContainer);
        questUI.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;

        activeQuestUI[quest.questName] = questUI;
    }

    public void CompleteQuestUI(string questName)
    {
        if (activeQuestUI.ContainsKey(questName))
        {
            Destroy(activeQuestUI[questName]);
            activeQuestUI.Remove(questName);
        }
    }
}

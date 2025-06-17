using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestUIController : MonoBehaviour
{
    public static QuestUIController Instance;

    [Header("References")]
    public GameObject questUIPrefab; // UI prefab
    public Transform questPanel;     // Panel içine UI öðeleri

    private List<GameObject> activeQuestUIs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RefreshQuestUI(List<Quest> quests)
    {
        // Önce eskileri temizle
        foreach (var ui in activeQuestUIs)
        {
            Destroy(ui);
        }
        activeQuestUIs.Clear();

        // Her bir görev için UI oluþtur
        foreach (var quest in quests)
        {
            GameObject newUI = Instantiate(questUIPrefab, questPanel);

            newUI.transform.Find("QuestName").GetComponent<TMP_Text>().text = quest.questName;
            newUI.transform.Find("QuestDesc").GetComponent<TMP_Text>().text = quest.description;

            // Ýkon varsa
            if (quest.requiredItem != null && quest.requiredItem.icon != null)
            {
                newUI.transform.Find("Icon").GetComponent<Image>().sprite = quest.requiredItem.icon;
            }
            else
            {
                newUI.transform.Find("Icon").gameObject.SetActive(false); // Ýkon yoksa gizle
            }

            activeQuestUIs.Add(newUI);
        }
    }
}

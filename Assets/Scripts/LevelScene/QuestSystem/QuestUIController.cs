using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class QuestUIController : MonoBehaviour
{
    public static QuestUIController Instance;

    [Header("References")]
    public GameObject questUIPrefab;
    public Transform questPanel;

    private List<GameObject> activeQuestUIs = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Open World Level")
        {
            Debug.Log("Open World Level loaded, refreshing Quest UI");

            questPanel = GameObject.Find("questPanel")?.transform;

            if (QuestManager.Instance != null && questPanel != null)
            {
                List<string> questIDs = QuestManager.Instance.GetAllAcquiredQuests();
                List<Quest> questsToShow = new List<Quest>();
                Quest[] allQuests = Resources.FindObjectsOfTypeAll<Quest>();

                foreach (string id in questIDs)
                {
                    if (QuestManager.Instance.IsQuestCompleted(id)) continue;

                    Quest quest = System.Array.Find(allQuests, q => q.questID == id);
                    if (quest != null)
                        questsToShow.Add(quest);
                }


                RefreshQuestUI(questsToShow);
            }
            else
            {
                Debug.LogWarning("QuestManager.Instance or questPanel not found.");
            }
        }
    }

    public void RefreshQuestUI(List<Quest> quests)
    {
        foreach (var ui in activeQuestUIs)
            Destroy(ui);
        activeQuestUIs.Clear();

        foreach (var quest in quests)
        {
            GameObject newUI = Instantiate(questUIPrefab, questPanel);

            newUI.transform.Find("QuestName").GetComponent<TMP_Text>().text = quest.questName;
            newUI.transform.Find("QuestDesc").GetComponent<TMP_Text>().text = quest.description;

            if (quest.requiredItem != null && quest.requiredItem.icon != null)
            {
                newUI.transform.Find("Icon").GetComponent<Image>().sprite = quest.requiredItem.icon;
            }
            else
            {
                newUI.transform.Find("Icon").gameObject.SetActive(false);
            }

            activeQuestUIs.Add(newUI);
        }
    }
}

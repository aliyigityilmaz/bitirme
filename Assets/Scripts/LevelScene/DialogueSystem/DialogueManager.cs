using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;
    public GameObject responsePanel;
    public Transform responseContainer;
    public GameObject responsePrefab;

    public Sprite normalIcon;
    public Sprite endIcon;
    public Sprite questIcon;

    private Queue<DialogueEntry> dialogueQueue;
    private PlayerController player;
    private QuestGiver currentQuestGiver;

    private List<GameObject> responseButtons = new List<GameObject>();
    private int selectedResponseIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        dialoguePanel.SetActive(false);
        responsePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (responsePanel.activeSelf)
        {
            // W veya S ile seçenekleri değiştir
            if (Input.GetKeyDown(KeyCode.W)) ChangeSelectedResponse(-1);
            if (Input.GetKeyDown(KeyCode.S)) ChangeSelectedResponse(1);

            // E ile seçimi onayla
            if (Input.GetKeyDown(KeyCode.E)) HandleResponseSelection();
        }
        else
        {
            // E ile sıradaki cümleye geç
            if (Input.GetKeyDown(KeyCode.E)) ShowNextSentence();
        }
    }

    public void StartDialogue(DialogueData dialogue, QuestGiver questGiver = null)
    {
        if (dialogue == null || dialogue.dialogueEntries.Length == 0) return;

        currentQuestGiver = questGiver;
        dialogueQueue = new Queue<DialogueEntry>(dialogue.dialogueEntries);
        dialoguePanel.SetActive(true);
        responsePanel.SetActive(false);

        nameText.text = dialogue.npcName ?? "";
        player.isTalking = true;

        player.StartTalking(); // Oyuncunun konu��ma durumunu ba��lat

        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueEntry currentEntry = dialogueQueue.Dequeue();
        dialogueText.text = currentEntry.dialogueLine;

        if (currentEntry.responses.Length > 0)
        {
            ShowResponses(currentEntry.responses);
        }
        else
        {
            responsePanel.SetActive(false);
        }
    }

    private void ShowResponses(DialogueResponse[] responses)
    {
        responsePanel.SetActive(true);

        foreach (Transform child in responseContainer)
        {
            Destroy(child.gameObject);
        }

        responseButtons.Clear();
        selectedResponseIndex = 0; // İlk yanıtı seçili yap

        for (int i = 0; i < responses.Length; i++)
        {
            GameObject responseButton = Instantiate(responsePrefab, responseContainer);
            responseButtons.Add(responseButton);

            TextMeshProUGUI buttonText = responseButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = responses[i].responseText;

            Image iconImage = responseButton.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = GetIconForResponse(responses[i].responseType);

            int index = i;
            responseButton.GetComponent<Button>().onClick.AddListener(() => HandleResponse(responses[index]));

            buttonText.color = (i == selectedResponseIndex) ? Color.yellow : Color.white;
        }
    }

    private void ChangeSelectedResponse(int direction)
    {
        if (responseButtons.Count == 0) return;

        // Önceki seçili butonu beyaz yap
        responseButtons[selectedResponseIndex].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        // Yeni seçili indeksi belirle
        selectedResponseIndex += direction;

        if (selectedResponseIndex < 0) selectedResponseIndex = responseButtons.Count - 1;
        if (selectedResponseIndex >= responseButtons.Count) selectedResponseIndex = 0;

        // Yeni seçili butonu sarıya boyayalım
        responseButtons[selectedResponseIndex].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
    }

    private void HandleResponseSelection()
    {
        if (responseButtons.Count == 0) return;
        responseButtons[selectedResponseIndex].GetComponent<Button>().onClick.Invoke();
    }

    private Sprite GetIconForResponse(ResponseType type)
    {
        switch (type)
        {
            case ResponseType.Normal:
                return normalIcon;
            case ResponseType.End:
                return endIcon;
            case ResponseType.AcceptQuest:
                return questIcon;
            default:
                return null;
        }
    }

    private void HandleResponse(DialogueResponse response)
    {
        switch (response.responseType)
        {
            case ResponseType.Normal:
                ShowNextSentence();
                break;
            case ResponseType.End:
                EndDialogue();
                break;
            // AcceptQuest durumunda:
            case ResponseType.AcceptQuest:
                if (currentQuestGiver != null && response.assignedQuest != null)
                {
                    QuestManager.Instance.AddQuest(response.assignedQuest);
                    currentQuestGiver.MarkQuestGiven(); // Buraya ekle
                    StartDialogue(currentQuestGiver.questGivenDialogue, currentQuestGiver);
                }
                else
                {
                    ShowNextSentence();
                }
                break;
            case ResponseType.CompleteQuest:
                if (response.assignedQuest != null)
                {
                    if (QuestManager.Instance.IsQuestCompleted(response.assignedQuest))
                    {
                        Debug.Log("Quest Completed!");
                        // Burada ödül vs. ekleyebilirsin.
                    }
                    else
                    {
                        Debug.Log("Quest not completed yet.");
                        return;
                    }
                }
                EndDialogue();
                break;
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        responsePanel.SetActive(false);
        player.isTalking = false;
        currentQuestGiver = null;
        player.EndTalking(); // Oyuncunun konu��ma durumunu durdur
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choiceContainer;
    public GameObject choicePrefab;

    private DialogueLine[] lines;
    private int index;
    private bool isTyping;

    private Coroutine typingCoroutine;

    private List<GameObject> currentChoices = new List<GameObject>();
    private int selectedChoiceIndex = 0;
    private bool awaitingChoice = false;

    public bool IsDialogueActive { get; private set; }
    private bool justClosedDialogue = false;
    public bool JustClosedDialogue => justClosedDialogue;

    [Header("Choice Icons")]
    public Sprite normalIcon;
    public Sprite endIcon;
    public Sprite acceptQuestIcon;
    // ileride: public Sprite eventIcon; vs...
    private string currentNPCName; // Bu deðiþkeni sýnýf düzeyine ekle

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!dialogueUI.activeSelf) return;

        if (awaitingChoice)
        {
            HandleChoiceInput();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index].text;
                isTyping = false;
                ShowChoicesIfAny();
            }
            else if (lines[index].choices == null || lines[index].choices.Length == 0)
            {
                index++;
                ShowNextLine();
            }
        }
    }

    public void StartDialogue(string npcName, DialogueLine[] dialogueLines)
    {
        if (IsDialogueActive) return;

        currentNPCName = npcName; // Burada kaydet
        lines = dialogueLines;
        index = 0;
        IsDialogueActive = true;
        dialogueUI.SetActive(true);

        PlayerController.Instance.StartTalking();
        ShowNextLine();
    }




    private void ShowNextLine()
    {
        ClearChoices();

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        string speaker = string.IsNullOrEmpty(lines[index].speakerName) ? currentNPCName : lines[index].speakerName;
        npcNameText.text = speaker;


        typingCoroutine = StartCoroutine(TypeLine(lines[index].text));
    }


    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
        ShowChoicesIfAny();
    }

    private void ShowChoicesIfAny()
    {
        var choices = lines[index].choices;
        if (choices != null && choices.Length > 0)
        {
            awaitingChoice = true;
            for (int i = 0; i < choices.Length; i++)
            {
                var choiceObj = Instantiate(choicePrefab, choiceContainer.transform);

                // Text bileþenini al
                var text = choiceObj.transform.Find("Answer").GetComponent<TextMeshProUGUI>();
                text.text = choices[i].choiceText;

                // Icon bileþenini al
                var iconImage = choiceObj.transform.Find("Icon").GetComponent<Image>();

                // Ýkonu seç
                switch (choices[i].choiceType)
                {
                    case ChoiceType.Normal:
                        iconImage.sprite = normalIcon;
                        break;
                    case ChoiceType.End:
                        iconImage.sprite = endIcon;
                        break;
                    case ChoiceType.AcceptQuest:
                        iconImage.sprite = acceptQuestIcon;
                        break;

                    default:
                        iconImage.enabled = false;
                        break;
                }

                // Buton listener
                int capturedIndex = i;
                choiceObj.GetComponent<Button>().onClick.AddListener(() => SelectChoice(capturedIndex));
                currentChoices.Add(choiceObj);
            }

            HighlightChoice(0);
        }
    }


    private void HandleChoiceInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            HighlightChoice((selectedChoiceIndex - 1 + currentChoices.Count) % currentChoices.Count);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            HighlightChoice((selectedChoiceIndex + 1) % currentChoices.Count);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SelectChoice(selectedChoiceIndex);
        }
    }

    private void HighlightChoice(int newIndex)
    {
        for (int i = 0; i < currentChoices.Count; i++)
        {
            var text = currentChoices[i].GetComponentInChildren<TextMeshProUGUI>();
            text.color = (i == newIndex) ? Color.yellow : Color.white;
        }
        selectedChoiceIndex = newIndex;
    }

    private void SelectChoice(int indexSelected)
    {
        var choice = lines[index].choices[indexSelected];

        switch (choice.choiceType)
        {
            case ChoiceType.End:
                EndDialogue();
                break;
            case ChoiceType.AcceptQuest:
                if (choice.quest != null)
                    QuestManager.Instance.AddQuest(choice.quest);
                EndDialogue();
                break;
            case ChoiceType.CompleteQuest:
                if (choice.quest != null)
                {
                    QuestManager.Instance.CompleteQuest(choice.quest);
                    QuestManager.Instance.PrintActiveQuests(); // Opsiyonel
                }
                EndDialogue();
                break;
            case ChoiceType.Normal:
            default:
                index++;
                awaitingChoice = false;
                ShowNextLine();
                break;
        }
    }


    private void ClearChoices()
    {
        foreach (var obj in currentChoices)
            Destroy(obj);
        currentChoices.Clear();
        awaitingChoice = false;
    }

    private void EndDialogue()
    {
        ClearChoices();
        dialogueUI.SetActive(false);
        IsDialogueActive = false;
        PlayerController.Instance.EndTalking();
        justClosedDialogue = true;
        StartCoroutine(ResetJustClosedFlag());
    }

    IEnumerator ResetJustClosedFlag()
    {
        yield return new WaitForSeconds(0.2f); // 0.2 saniye sonra tekrar konuþma yapýlabilir
        justClosedDialogue = false;
    }
}

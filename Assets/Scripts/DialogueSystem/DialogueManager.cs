using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject dialoguePanel;

    private Queue<string> dialogueQueue;
    private bool isDialogueActive = false;
    private PlayerController player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>(); // Oyuncuyu bul
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowNextSentence();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (isDialogueActive) return;

        dialogueQueue = new Queue<string>(dialogue.dialogueLines);
        dialoguePanel.SetActive(true);
        if (dialogue.npcName!=null)
        {
            nameText.text = dialogue.npcName;
        }
        else
        {
            nameText.text = "";
        }
        isDialogueActive = true;
        if (player != null)
        {
            player.isTalking = true; // Oyuncu konuþma sýrasýnda hareket edemesin
        }
        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogueQueue.Dequeue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        if (player != null)
        {
            player.isTalking = false; // Diyalog bitince oyuncu hareket edebilir
        }
    }
}

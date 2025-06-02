using UnityEngine;

public class QuestGiver : Interactable
{
    public DialogueData initialDialogue;
    public DialogueData questGivenDialogue;
    public DialogueData questCompleteDialogue;
    public Quest questToGive;

    public bool isQuestGiven = false;
    public bool isQuestCompleted = false;

    public GameObject questIndicator; // �nlem objesi

    private void Start()
    {
        interactableType = InteractableType.QuestGiver;
        UpdateQuestIndicator();
    }

    private void Update()
    {
        UpdateQuestIndicator();
    }

    public override void Interact()
    {
        if (questToGive == null || !questToGive.IsAvailable())
        {
            // �n ko�ul g�rev tamamlanmam��, konu�ma yok.
            return;
        }

        if (!isQuestGiven)
        {
            DialogueManager.Instance.StartDialogue(initialDialogue, this);
        }
        else if (QuestManager.Instance.IsQuestCompleted(questToGive) && !isQuestCompleted)
        {
            DialogueManager.Instance.StartDialogue(questCompleteDialogue, this);
            isQuestCompleted = true;
            // Burada �d�l verebilirsin
        }
        else
        {
            DialogueManager.Instance.StartDialogue(questGivenDialogue, this);
        }
    }

    public void MarkQuestGiven()
    {
        isQuestGiven = true;
    }

    private void UpdateQuestIndicator()
    {
        if (questIndicator == null || questToGive == null)
            return;

        if (isQuestCompleted)
        {
            questIndicator.SetActive(false); // G�rev bitti, �nlem kalks�n
        }
        else if (!isQuestGiven && questToGive.IsAvailable())
        {
            questIndicator.SetActive(true); // G�rev al�nabilir, �nlem g�ster
        }
        else if (isQuestGiven && QuestManager.Instance.IsQuestCompleted(questToGive) && !isQuestCompleted)
        {
            questIndicator.SetActive(true); // Tamamlanmaya haz�r g�rev
        }
        else
        {
            questIndicator.SetActive(false); // Her durumda gizle
        }
    }
}

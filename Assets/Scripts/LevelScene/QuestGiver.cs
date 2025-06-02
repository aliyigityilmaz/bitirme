using UnityEngine;

public class QuestGiver : Interactable
{
    public DialogueData initialDialogue;
    public DialogueData questGivenDialogue;
    public DialogueData questCompleteDialogue;
    public Quest questToGive;

    public bool isQuestGiven = false;
    public bool isQuestCompleted = false;

    public GameObject questIndicator; // Ünlem objesi

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
            // Ön koþul görev tamamlanmamýþ, konuþma yok.
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
            // Burada ödül verebilirsin
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
            questIndicator.SetActive(false); // Görev bitti, ünlem kalksýn
        }
        else if (!isQuestGiven && questToGive.IsAvailable())
        {
            questIndicator.SetActive(true); // Görev alýnabilir, ünlem göster
        }
        else if (isQuestGiven && QuestManager.Instance.IsQuestCompleted(questToGive) && !isQuestCompleted)
        {
            questIndicator.SetActive(true); // Tamamlanmaya hazýr görev
        }
        else
        {
            questIndicator.SetActive(false); // Her durumda gizle
        }
    }
}

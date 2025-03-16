using UnityEngine;

public class QuestGiver : Interactable
{
    public DialogueData initialDialogue;
    public DialogueData questGivenDialogue;
    public DialogueData questCompletedDialogue;
    public Quest quest; // Verilecek görev

    public bool isQuestGiven = false;

    public override void Interact()
    {
        if (!isQuestGiven)
        {
            DialogueManager.Instance.StartDialogue(initialDialogue, this);
        }
        else if (!QuestManager.Instance.IsQuestCompleted(quest.questName))
        {
            DialogueManager.Instance.StartDialogue(questGivenDialogue, this);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(questCompletedDialogue, this);
        }
    }

    // Görev ekleme metodu (DialogueManager burayý çaðýracak)
    public void GiveQuest()
    {
        if (!isQuestGiven && quest != null)
        {
            QuestManager.Instance.AddQuest(quest);
            isQuestGiven = true;
            Debug.Log("Quest Given: " + quest.questName);
        }
    }
}

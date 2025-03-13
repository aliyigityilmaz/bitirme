using UnityEngine;

public class QuestGiver : Interactable
{
    public DialogueData initialDialogue; // G�rev verilmeden �nceki konu�ma
    public DialogueData questGivenDialogue; // G�rev verildi�inde ama tamamlanmad���nda
    public DialogueData questCompletedDialogue; // G�rev tamamland�ktan sonraki konu�ma
    public Quest quest; // NPC'nin verdi�i g�rev

    private bool isQuestGiven = false;

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

    public void GiveQuest()
    {
        if (!isQuestGiven)
        {
            isQuestGiven = true;
            QuestManager.Instance.AddQuest(quest);
        }
    }
}

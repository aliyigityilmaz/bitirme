using UnityEngine;

public class QuestGiver : Interactable
{
    public DialogueData initialDialogue; // Görev verilmeden önceki konuþma
    public DialogueData questGivenDialogue; // Görev verildiðinde ama tamamlanmadýðýnda
    public DialogueData questCompletedDialogue; // Görev tamamlandýktan sonraki konuþma
    public Quest quest; // NPC'nin verdiði görev

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

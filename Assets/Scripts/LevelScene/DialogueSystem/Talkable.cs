using System.Collections.Generic;
using UnityEngine;

public class TalkableNPC : Interactable
{
    public string npcName;
    public DialogueLine[] dialogueLines;
    public Quest quest; // Yeni eklendi

    private bool hasInteracted = false;

    [Header("Bu NPC'nin g�revinde �ld�r�lmesi gereken d��manlar")]
    public List<GameObject> requiredEnemies; // Sahnedeki d��manlar


    private void Start()
    {
        interactableType = InteractableType.Talkable;
    }

    public override void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive || DialogueManager.Instance.JustClosedDialogue) return;
        if (isOneTimeInteraction && hasInteracted) return;

        if (isOneTimeInteraction)
            hasInteracted = true;

        DialogueManager.Instance.StartDialogue(npcName, GetFilteredDialogue());
    }

    private DialogueLine[] GetFilteredDialogue()
    {
        var modifiedLines = new List<DialogueLine>();

        foreach (var line in dialogueLines)
        {
            if (line.choices != null && line.choices.Length > 0)
            {
                var filteredChoices = new List<DialogueChoice>();

                foreach (var choice in line.choices)
                {
                    bool skip = false;

                    if (choice.choiceType == ChoiceType.AcceptQuest && choice.quest != null)
                    {
                        if (QuestManager.Instance.HasQuest(choice.quest.questID))
                            skip = true;
                    }

                    if (choice.choiceType == ChoiceType.CompleteQuest && choice.quest != null)
                    {
                        if (requiredEnemies != null && requiredEnemies.Count > 0)
                        {
                            bool anyAlive = requiredEnemies.Exists(enemy => enemy != null && enemy.activeSelf);
                            if (anyAlive)
                            {
                                skip = true;
                            }
                        }
                        else if (!QuestManager.Instance.CanCompleteQuest(choice.quest))
                        {
                            skip = true;
                        }
                    }




                    if (!skip)
                        filteredChoices.Add(choice);
                }

                modifiedLines.Add(new DialogueLine
                {
                    text = line.text,
                    choices = filteredChoices.ToArray()
                });
            }
            else
            {
                modifiedLines.Add(line);
            }
        }

        return modifiedLines.ToArray();
    }

}


[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;

    public DialogueChoice[] choices; // null veya bo�sa cevap yok
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public ChoiceType choiceType;
    public Quest quest; // Hem accept hem complete i�in
}


public enum ChoiceType
{
    Normal,
    End,
    AcceptQuest,
    CompleteQuest //  Yeni tamamla se�ene�i
}


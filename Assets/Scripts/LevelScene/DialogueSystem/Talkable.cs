using System.Collections.Generic;
using UnityEngine;

public class TalkableNPC : Interactable
{
    public string npcName;
    public DialogueLine[] dialogueLines;
    public Quest quest; // Yeni eklendi

    private bool hasInteracted = false;

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
        // G�rev daha �nce al�nd�ysa AcceptQuest tipindeki cevaplar� filtrele
        if (quest == null || !QuestManager.Instance.HasQuest(quest.questID))
            return dialogueLines;

        var modifiedLines = new List<DialogueLine>();
        foreach (var line in dialogueLines)
        {
            if (line.choices != null && line.choices.Length > 0)
            {
                var filteredChoices = new List<DialogueChoice>();
                foreach (var choice in line.choices)
                {
                    if (choice.choiceType == ChoiceType.AcceptQuest && QuestManager.Instance.HasQuest(quest.questID))
                        continue;

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
    public Quest questToAccept; // AcceptQuest i�in atan�r
}


public enum ChoiceType
{
    Normal,
    End,
    AcceptQuest // Yeni eklendi
}


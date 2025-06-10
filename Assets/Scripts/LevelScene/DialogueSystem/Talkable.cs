using UnityEngine;

public class TalkableNPC : Interactable
{
    public string npcName;
    public DialogueLine[] dialogueLines;

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

        DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
    }



}


[System.Serializable]
public class DialogueLine
{
    [TextArea]
    public string text;

    public DialogueChoice[] choices; // null veya boþsa cevap yok
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public ChoiceType choiceType;
}

public enum ChoiceType
{
    Normal,
    End
}

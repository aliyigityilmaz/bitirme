using UnityEngine;

public class TalkableNPC : Interactable
{
    public DialogueData dialogue;

    public override void Interact()
    {
        if (dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Talkable,
        Pickup,
        Door,
        QuestGiver
    }

    public InteractableType interactableType;

    public abstract void Interact();
}


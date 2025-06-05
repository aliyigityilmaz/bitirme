using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Talkable,
        Pickup,
        Collectible,
        Chest,
        Door,
        QuestGiver,
        Enemy
    }

    public InteractableType interactableType;

    public abstract void Interact();
}


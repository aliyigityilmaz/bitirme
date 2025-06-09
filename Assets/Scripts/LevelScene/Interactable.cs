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
        Torch,
        RotatableStone,
        Enemy
    }

    public InteractableType interactableType;

    [Header("Interaction Settings")]
    public bool isOneTimeInteraction = true; // Sadece bir kere mi etkileþim?

    public abstract void Interact();
}

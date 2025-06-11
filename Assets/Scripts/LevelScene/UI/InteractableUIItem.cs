using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUIItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text descriptionText;

    public Sprite talkIcon, chestIcon, doorIcon, collectibleIcon, torchIcon, stoneIcon, enemyIcon, questGiverIcon; // vs
    private Interactable currentInteractable;

    public void Setup(Interactable interactable)
    {
        currentInteractable = interactable;
        switch (interactable.interactableType)
        {
            case Interactable.InteractableType.Talkable:
                iconImage.sprite = talkIcon;
                descriptionText.text = "Talk with "+ interactable.GetComponent<TalkableNPC>().npcName;
                break;
            case Interactable.InteractableType.Chest:
                iconImage.sprite = chestIcon;
                descriptionText.text = "Open the chest";
                break;
            case Interactable.InteractableType.Door:
                iconImage.sprite = doorIcon;
                descriptionText.text = "Open the door";
                break;
            case Interactable.InteractableType.Collectible:
                iconImage.sprite = collectibleIcon;
                descriptionText.text = "Pick up " + interactable.GetComponent<CollectiblePickup>().itemData.itemName;
                break;
            case Interactable.InteractableType.Torch:
                iconImage.sprite = torchIcon;
                descriptionText.text = "Light the torch";
                break;
            case Interactable.InteractableType.RotatableStone:
                iconImage.sprite = stoneIcon;
                descriptionText.text = "Rotate the stone";
                break;
            case Interactable.InteractableType.Enemy:
                iconImage.sprite = enemyIcon;
                descriptionText.text = "Fight the enemy";
                break;
            case Interactable.InteractableType.QuestGiver:
                iconImage.sprite = questGiverIcon;
                descriptionText.text = "Talk with " + interactable.GetComponent<TalkableNPC>().npcName;
                break;
                // Diðer türler...
        }

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners(); // Önceki baðlantýlarý temizle
            button.onClick.AddListener(() =>
            {
                if (currentInteractable != null)
                    currentInteractable.Interact();
            });
        }
    }
}

using UnityEngine;

public class CollectiblePickup : Interactable
{
    [Header("Item to Give")]
    public InventoryItemData itemData;
    public int quantity = 1;

    private bool hasCollected = false;

    private void Reset()
    {
        interactableType = InteractableType.Pickup;
    }

    public override void Interact()
    {
        if (hasCollected || itemData == null) return;

        hasCollected = true;

        BackpackManager.Instance.AddItem(itemData, quantity);
        FloatingTextSpawner.Instance.ShowMessage(
            $"You have collected {itemData.itemName}!",
            Color.white
        );
        // Ses, VFX, animasyon gibi þeyler burada olabilir
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (InteractableUIManager.Instance != null)
        {
            InteractableUIManager.Instance.HideInteractable(this);
        }
    }

}

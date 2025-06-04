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

        // Ses, VFX, animasyon gibi �eyler burada olabilir
        Destroy(gameObject);
    }
}

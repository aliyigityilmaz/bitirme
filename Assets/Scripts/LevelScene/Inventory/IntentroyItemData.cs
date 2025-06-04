using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType type;
    public int defaultQuantity = 1;

    public bool IsStackable()
    {
        return type == ItemType.Collectible;
    }
}

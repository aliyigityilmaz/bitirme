using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType type;
    public int defaultQuantity = 1;

    public bool IsStackable()
    {
        // Collectible, Consumable ve MobDrop türleri stackable olacak
        return type == ItemType.Collectible || type == ItemType.Consumable || type == ItemType.MobDrop;
    }
}

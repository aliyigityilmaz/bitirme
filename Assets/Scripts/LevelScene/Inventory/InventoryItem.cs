using UnityEngine;

public enum ItemType
{
    CharacterItem = 0,
    QuestItem = 1,
    Consumable = 2,
    MobDrop = 3,
    Collectible = 4
}


[System.Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    [HideInInspector] public int quantity = 1;

    public string ItemName => data.itemName;
    public string Description => data.description;
    public Sprite Icon => data.icon;
    public ItemType Type => data.type;

    public bool IsStackable() => data.IsStackable();
}



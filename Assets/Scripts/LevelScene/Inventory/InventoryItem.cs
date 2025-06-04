using UnityEngine;

public enum ItemType
{
    Collectible,
    CharacterItem,
    QuestItem
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



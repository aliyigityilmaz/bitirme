using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    [Header("Tamamlamak i�in gerekenler")]
    public InventoryItemData requiredItem; // ?? gerekli item
    public int requiredItemCount = 1;       // ?? ka� tane gerekti�i
}

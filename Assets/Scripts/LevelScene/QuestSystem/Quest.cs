using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    [Header("Tamamlamak için gerekenler")]
    public InventoryItemData requiredItem; // ?? gerekli item
    public int requiredItemCount = 1;       // ?? kaç tane gerektiði
}

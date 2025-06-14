using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;

    [Header("Tamamlamak için gerekenler")]
    public InventoryItemData requiredItem;
    public int requiredItemCount = 1;       

}

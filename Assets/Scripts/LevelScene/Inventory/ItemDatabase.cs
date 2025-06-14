using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<InventoryItemData> allItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static InventoryItemData GetItemByID(string id)
    {
        return Instance.allItems.FirstOrDefault(item => item.id == id);
    }
}

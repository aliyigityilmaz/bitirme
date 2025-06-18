using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BackpackManager : MonoBehaviour
{
    public static BackpackManager Instance;

    [Header("UI")]
    public GameObject panel;
    public Transform contentGrid;
    public GameObject itemSlotPrefab;
    public Button useButton;

    [Header("Frame Styles")]
    public Sprite redFrameNormal;
    public Sprite greenFrameNormal;
    public Sprite grayFrameNormal;

    [Header("Selected Item UI")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemType;
    public Image selectedItemTypeIcon;
    public Image selectedItemIcon;
    public TextMeshProUGUI selectedItemDescription;

    [Header("Selected Item UI")]
    public GameObject selectedItemPanel; // Tüm seçili item alanlarýnýn parent'ý

    public Sprite selectedItemQuestIcon;
    public Sprite selectedItemCharacterIcon;
    public Sprite selectedItemCollectibleIcon;
    public Sprite selectedItemConsumableIcon;
    public Sprite selectedItemMobDropIcon;

    private List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadBackpack();

        selectedItemPanel.SetActive(false); // UI baþlangýçta gizli
    }

    public void ToggleBackpack()
    {
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf)
        {
            RefreshUI();
        }
        else
        {
            selectedItemPanel.SetActive(false); // Çanta kapanýnca sýfýrla
        }
    }


    public void AddItem(InventoryItemData newItemData, int quantity = 1)
    {
        if (newItemData.IsStackable())
        {
            var existing = items.FirstOrDefault(i => i.data == newItemData);
            if (existing != null)
            {
                existing.quantity += quantity;
            }
            else
            {
                items.Add(new InventoryItem { data = newItemData, quantity = quantity });
            }
        }
        else
        {
            for (int i = 0; i < quantity; i++)
            {
                items.Add(new InventoryItem { data = newItemData, quantity = 1 });
            }
        }

        RefreshUI();
    }


    void RefreshUI()
    {
        foreach (Transform child in contentGrid)
            Destroy(child.gameObject);

        var sorted = items.OrderBy(i =>
        {
            switch (i.Type)
            {
                case ItemType.CharacterItem: return 0;
                case ItemType.QuestItem: return 1;
                case ItemType.Consumable: return 2;
                case ItemType.MobDrop: return 3;
                case ItemType.Collectible: return 4;
                default: return 5;
            }
        }).ToList();


        foreach (var item in sorted)
        {
            GameObject slot = Instantiate(itemSlotPrefab, contentGrid);
            var icon = slot.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = item.Icon;

            var frame = slot.transform.Find("Background")?.GetComponent<Image>();
            var button = slot.GetComponent<Button>();
            var controller = slot.AddComponent<ItemSlotController>();
            controller.Setup(item);

            switch (item.Type)
            {
                case ItemType.CharacterItem:
                    frame.sprite = redFrameNormal;
                    break;
                case ItemType.QuestItem:
                    frame.sprite = greenFrameNormal;
                    break;
                case ItemType.Collectible:
                    frame.sprite = grayFrameNormal;
                    break;
                case ItemType.Consumable:
                    frame.sprite = greenFrameNormal;
                    break;
                case ItemType.MobDrop:
                    frame.sprite = grayFrameNormal;
                    break;
            }

            // Sayý gösterimi
            var quantityText = slot.transform.Find("Count")?.GetComponent<TextMeshProUGUI>();
            if ((item.Type == ItemType.Collectible || item.Type == ItemType.Consumable || item.Type == ItemType.MobDrop || item.Type == ItemType.QuestItem) && item.quantity > 1)
            {
                quantityText.text = item.quantity.ToString();
                quantityText.gameObject.SetActive(true);
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }
        }

    }

    public void ShowSelectedItem(InventoryItem item)
    {
        selectedItemPanel.SetActive(true);
        // Temel bilgileri güncelle
        selectedItemName.text = item.ItemName;
        selectedItemDescription.text = item.Description;
        selectedItemIcon.sprite = item.Icon;

        // Tür metni ve ikonunu ayarla
        switch (item.Type)
        {
            case ItemType.CharacterItem:
                useButton.interactable = false;
                useButton.onClick.RemoveAllListeners();
                selectedItemType.text = "Character Item";
                if (selectedItemCharacterIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemCharacterIcon; // veya özel bir ikon
                }
                break;

            case ItemType.QuestItem:
                useButton.interactable = false;
                useButton.onClick.RemoveAllListeners();
                selectedItemType.text = "Quest Item";
                if (selectedItemQuestIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemQuestIcon; // veya özel bir ikon
                }
                break;

            case ItemType.Collectible:
                useButton.interactable = false;
                useButton.onClick.RemoveAllListeners();
                selectedItemType.text = "Collectible";
                if (selectedItemCollectibleIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemCollectibleIcon; // veya özel bir ikon
                }
                break;
            case ItemType.Consumable:
                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(() => UseSelectedItem(item));
                selectedItemType.text = "Consumable";
                if (selectedItemConsumableIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemConsumableIcon; // veya özel bir ikon
                }
                break;
            case ItemType.MobDrop:
                useButton.interactable = false;
                useButton.onClick.RemoveAllListeners();
                selectedItemType.text = "Mob Drops";
                if (selectedItemMobDropIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemMobDropIcon; // veya özel bir ikon
                }
                break;
        }
    }
    private void UseSelectedItem(InventoryItem item)
    {
        RemoveItem(item.data, 1);
        selectedItemPanel.SetActive(false); // UI'yi kapat
    }
    public bool HasItem(InventoryItemData itemData, int quantity)
    {
        var item = items.FirstOrDefault(i => i.data == itemData);
        return item != null && item.quantity >= quantity;
    }

    public void RemoveItem(InventoryItemData itemData, int quantity)
    {
        var item = items.FirstOrDefault(i => i.data == itemData);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                items.Remove(item);
            }
            RefreshUI();
        }
    }



    public int GetItemCount(InventoryItemData itemData) // YENÝ
    {
        foreach (var item in items)
        {
            if (item.data == itemData)
                return item.quantity;
        }
        return 0;
    }

    public void SaveBackpack()
    {
        BackpackSaveData saveData = new BackpackSaveData();
        saveData.items = new List<InventoryItemSerialized>();

        foreach (var item in items)
        {
            saveData.items.Add(new InventoryItemSerialized
            {
                itemID = item.data.id, // her itemData'da benzersiz `id` olduðuna emin ol
                quantity = item.quantity
            });
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("BackpackData", json);
        PlayerPrefs.Save();
    }

    public void LoadBackpack()
    {
        if (!PlayerPrefs.HasKey("BackpackData")) return;

        string json = PlayerPrefs.GetString("BackpackData");
        BackpackSaveData saveData = JsonUtility.FromJson<BackpackSaveData>(json);

        items.Clear();
        foreach (var savedItem in saveData.items)
        {
            InventoryItemData itemData = ItemDatabase.GetItemByID(savedItem.itemID);
            if (itemData != null)
            {
                items.Add(new InventoryItem { data = itemData, quantity = savedItem.quantity });
            }
        }

        RefreshUI();
    }
    public bool HasEnoughItem(InventoryItemData itemData, int requiredCount)
    {
        var existingItem = items.FirstOrDefault(i => i.data == itemData);
        return existingItem != null && existingItem.quantity >= requiredCount;
    }

}

[System.Serializable]
public class BackpackSaveData
{
    public List<InventoryItemSerialized> items;
}

[System.Serializable]
public class InventoryItemSerialized
{
    public string itemID;
    public int quantity;
}


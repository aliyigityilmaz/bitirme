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

    public Sprite selectedItemQuestIcon;
    public Sprite selectedItemCharacterIcon;
    public Sprite selectedItemCollectibleIcon;


    private List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleBackpack()
    {
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf)
            RefreshUI();
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
                case ItemType.Collectible: return 2;
                default: return 3;
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
            }

            // Sayý gösterimi
            var quantityText = slot.transform.Find("Count")?.GetComponent<TextMeshProUGUI>();
            if (item.Type == ItemType.Collectible && item.quantity > 1)
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
        // Temel bilgileri güncelle
        selectedItemName.text = item.ItemName;
        selectedItemDescription.text = item.Description;
        selectedItemIcon.sprite = item.Icon;

        // Tür metni ve ikonunu ayarla
        switch (item.Type)
        {
            case ItemType.CharacterItem:
                selectedItemType.text = "Character Item";
                if (selectedItemCharacterIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemCharacterIcon; // veya özel bir ikon
                }
                break;

            case ItemType.QuestItem:
                selectedItemType.text = "Quest Item";
                if (selectedItemQuestIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemQuestIcon; // veya özel bir ikon
                }
                break;

            case ItemType.Collectible:
                selectedItemType.text = "Collectible";
                if (selectedItemCollectibleIcon != null)
                {
                    selectedItemTypeIcon.sprite = selectedItemCollectibleIcon; // veya özel bir ikon
                }
                break;
        }
    }

    public bool CheckItem(string itemName)
    {
        return items.Any(item => item.ItemName == itemName);
    }

    public bool RemoveItem(string itemName)
    {
        var targetItem = items.FirstOrDefault(item => item.ItemName == itemName);
        if (targetItem != null)
        {
            if (targetItem.IsStackable())
            {
                targetItem.quantity--;
                if (targetItem.quantity <= 0)
                    items.Remove(targetItem);
            }
            else
            {
                items.Remove(targetItem);
            }

            RefreshUI();
            return true;
        }

        return false;
    }

}

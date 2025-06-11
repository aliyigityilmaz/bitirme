using UnityEngine;

public enum ChestUnlockType
{
    Free,       // Direkt açýlýr
    RequiresItem,
    RequiresEnemyKill
}

[System.Serializable]
public struct ChestReward
{
    public InventoryItemData itemData;
    public int quantity;
}

public class Chest : Interactable
{
    [Header("Items to Give")]
    public ChestReward[] itemRewards;


    [Header("Chest Settings")]
    public ChestUnlockType unlockType = ChestUnlockType.Free;

    [Header("If Requires Item")]
    public InventoryItemData requiredItem;
    public bool consumeItem = true;

    [Header("If Requires Enemy Kill")]
    public GameObject[] enemiesToKill;

    private bool hasCollected = false;

    private void Reset()
    {
        interactableType = InteractableType.Chest;
    }

    public override void Interact()
    {
        if (hasCollected || itemRewards == null) return;

        if (!CanBeOpened())
            return; // CanBeOpened içinde mesaj gösterilecek

        hasCollected = true;

        foreach (var reward in itemRewards)
        {
            BackpackManager.Instance.AddItem(reward.itemData, reward.quantity);
        }

        // Ses, VFX, animasyon vb.
        Destroy(gameObject);
    }



    private bool CanBeOpened()
    {
        switch (unlockType)
        {
            case ChestUnlockType.Free:
                return true;

            case ChestUnlockType.RequiresItem:
                if (requiredItem == null)
                {
                    Debug.LogWarning("Gerekli item ayarlanmamýþ!");
                    return false;
                }

                if (BackpackManager.Instance.HasItem(requiredItem))
                {
                    if (consumeItem)
                        BackpackManager.Instance.RemoveItem(requiredItem);
                    return true;
                }
                else
                {
                    FloatingTextSpawner.Instance.ShowMessage("Bu sandýðý açmak için gereken item yok!", Color.red);
                    return false;
                }

            case ChestUnlockType.RequiresEnemyKill:
                foreach (GameObject enemy in enemiesToKill)
                {
                    if (enemy != null && enemy.activeSelf)
                    {
                        FloatingTextSpawner.Instance.ShowMessage("Tüm düþmanlar ölmeden bu sandýðý açamazsýn!", Color.red);
                        return false;
                    }
                }
                return true;

            default:
                return false;
        }
    }

    protected virtual void OnDestroy()
    {
        if (InteractableUIManager.Instance != null)
        {
            InteractableUIManager.Instance.HideInteractable(this);
        }
    }


}

using UnityEngine;

public enum ChestUnlockType
{
    Free,
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
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Reset()
    {
        interactableType = InteractableType.Chest;
    }

    public override void Interact()
    {
        if (hasCollected || itemRewards == null) return;

        if (!CanBeOpened())
            return;

        hasCollected = true;

        if (animator != null)
        {
            animator.SetTrigger("OpenChest");
        }

        StartCoroutine(OpenAndCollect());
    }

    private System.Collections.IEnumerator OpenAndCollect()
    {
        yield return new WaitForSeconds(2.5f); // animasyon süresi

        foreach (var reward in itemRewards)
        {
            BackpackManager.Instance.AddItem(reward.itemData, reward.quantity);
        }

        Destroy(gameObject); // sandýðý yok et
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

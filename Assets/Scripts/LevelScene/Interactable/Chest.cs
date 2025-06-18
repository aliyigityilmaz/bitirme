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

    public GameObject openVFX;
    private string generatedID;


    public GameObject unlockVFXPrefab;
    private GameObject unlockVFXInstance;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        generatedID = $"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}_Chest_{transform.position.x}_{transform.position.y}_{transform.position.z}";

        if (PlayerPrefs.GetInt("ChestOpened_" + generatedID, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        if (unlockType == ChestUnlockType.RequiresItem || unlockType == ChestUnlockType.RequiresEnemyKill)
        {
            if (unlockVFXPrefab != null)
            {
                unlockVFXInstance = Instantiate(unlockVFXPrefab, transform.position, Quaternion.identity, transform);
            }
        }
    }


    private void Reset()
    {
        interactableType = InteractableType.Chest;
    }

    public override void Interact()
    {
        if (hasCollected) return;

        if (!CanBeOpened())
            return;

        if (unlockVFXInstance != null)
            Destroy(unlockVFXInstance);

        AudioManager.Instance.PlaySFX(2); // Sand�k a�ma sesi

        if (itemRewards == null || itemRewards.Length == 0)
        {
            Debug.LogWarning("�d�l listesi bo�!");
            return;
        }

        hasCollected = true;
        PlayerPrefs.SetInt("ChestOpened_" + generatedID, 1);
        PlayerPrefs.Save();
        if (animator != null)
        {
            animator.SetTrigger("OpenChest");

            GameObject vfx = Instantiate(openVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 2.5f); // VFX'i 2 saniye sonra yok et
        }

        StartCoroutine(OpenAndCollect());
    }


    private System.Collections.IEnumerator OpenAndCollect()
    {
        yield return new WaitForSeconds(2.5f); // animasyon s�resi

        foreach (var reward in itemRewards)
        {
            BackpackManager.Instance.AddItem(reward.itemData, reward.quantity);

            // �d�l mesaj�
            FloatingTextSpawner.Instance.ShowMessage($"+{reward.quantity} {reward.itemData.itemName}", Color.yellow);

            yield return new WaitForSeconds(0.5f); // Her item aras�nda bekleme s�resi
        }

        Destroy(gameObject); // Sand��� yok et
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
                    Debug.LogWarning("Gerekli item ayarlanmam��!");
                    return false;
                }

                if (BackpackManager.Instance.HasItem(requiredItem, 1))
                {
                    if (consumeItem)
                        BackpackManager.Instance.RemoveItem(requiredItem, 1);
                    if (unlockVFXInstance != null)
                        Destroy(unlockVFXInstance);
                    return true;
                }
                else
                {
                    FloatingTextSpawner.Instance.ShowMessage("Bu sand��� a�mak i�in gereken item yok!", Color.red);
                    return false;
                }

            case ChestUnlockType.RequiresEnemyKill:
                if (enemiesToKill == null || enemiesToKill.Length == 0)
                {
                    Debug.LogWarning("Enemy listesi bo�, ama sand�k d��man gerektiriyor!");
                    return false;
                }

                foreach (GameObject enemy in enemiesToKill)
                {
                    if (enemy != null && enemy.activeInHierarchy)
                    {
                        Debug.Log($"D��man aktif: {enemy.name}");
                        FloatingTextSpawner.Instance.ShowMessage("T�m d��manlar �lmeden bu sand��� a�amazs�n!", Color.red);
                        return false;
                    }
                }
                if (unlockVFXInstance != null)
                    Destroy(unlockVFXInstance);
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

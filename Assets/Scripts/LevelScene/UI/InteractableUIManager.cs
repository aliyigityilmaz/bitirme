using System.Collections.Generic;
using UnityEngine;

public class InteractableUIManager : MonoBehaviour
{
    public static InteractableUIManager Instance;

    public Transform container; // Vertical Layout Group içeren panel
    public GameObject uiItemPrefab;

    private Dictionary<Interactable, GameObject> activeUIItems = new Dictionary<Interactable, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void ShowInteractable(Interactable interactable)
    {
        if (activeUIItems.ContainsKey(interactable)) return;

        GameObject uiItem = Instantiate(uiItemPrefab, container);
        InteractableUIItem item = uiItem.GetComponent<InteractableUIItem>();
        item.Setup(interactable);

        activeUIItems[interactable] = uiItem;

        // Otomatik olarak UI Selector tetiklensin
        if (InteractableUISelector.Instance != null)
        {
            InteractableUISelector.Instance.Invoke("UpdateButtonList", 0.01f); // Sonraki frame'de güncelle
        }
    }


    public void HideInteractable(Interactable interactable)
    {
        if (!activeUIItems.ContainsKey(interactable)) return;

        Destroy(activeUIItems[interactable]);
        activeUIItems.Remove(interactable);
    }

    public void ClearAll()
    {
        foreach (var kvp in activeUIItems)
        {
            Destroy(kvp.Value);
        }
        activeUIItems.Clear();
    }
}

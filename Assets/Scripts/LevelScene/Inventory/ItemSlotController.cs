using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemSlotController : MonoBehaviour, IPointerClickHandler
{
    private Vector3 originalScale;
    private bool isAnimating = false;
    private InventoryItem currentItem;

    public void Setup(InventoryItem item)
    {
        currentItem = item;
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;
        StartCoroutine(ClickAnimation());
        BackpackManager.Instance.ShowSelectedItem(currentItem);
    }

    private IEnumerator ClickAnimation()
    {
        isAnimating = true;

        Vector3 smallScale = originalScale * 0.9f;
        float duration = 0.1f;

        // Küçült
        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, smallScale, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // Büyüt
        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(smallScale, originalScale, t / duration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        isAnimating = false;
    }
}

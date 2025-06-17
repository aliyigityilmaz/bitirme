using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    public GameObject floatingTextPrefab;
    public Transform playerHead;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowMessage(string message, Color color)
    {
        if (floatingTextPrefab == null || playerHead == null)
        {
            Debug.LogWarning("FloatingTextSpawner: Prefab veya playerHead atanmadý!");
            return;
        }

        GameObject obj = Instantiate(floatingTextPrefab, playerHead.position + Vector3.up * 1.5f, Quaternion.identity);
        obj.GetComponent<FloatingText>().Setup(message, color);
        Destroy(obj, 3f); // 2 saniye sonra yok et
    }
}

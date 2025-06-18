using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager Instance;

    private Vector3 savedPosition;
    private bool shouldRestorePosition = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçiþlerinde kaybolmasýn
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Open World Level") // Ana sahne adýný gir
        {
            TryRestorePosition(PlayerController.Instance.gameObject);
        }
    }



    public void SavePosition(Vector3 pos)
    {
        savedPosition = pos;
        shouldRestorePosition = true;
    }

    public void TryRestorePosition(GameObject player)
    {
        if (shouldRestorePosition)
        {
            player.transform.position = savedPosition;
            shouldRestorePosition = false;
        }
    }
}

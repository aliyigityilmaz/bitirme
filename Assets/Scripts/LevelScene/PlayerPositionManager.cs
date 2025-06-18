using UnityEngine;
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(gameObject); // Sahne ge�i�lerinde kaybolmas�n
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Open World Level") // Ana sahne ad�n� gir
        {
            TryRestorePosition(PlayerController.Instance.gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Open World Level") // Ana sahne ad�n� gir
        {
      //      TryRestorePosition(PlayerController.Instance.gameObject);
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

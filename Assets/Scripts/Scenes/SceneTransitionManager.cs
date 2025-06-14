using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public string targetSpawnPointID;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSpawnPoint(string spawnID)
    {
        targetSpawnPointID = spawnID;
    }

    public string GetSpawnPoint()
    {
        return targetSpawnPointID;
    }
}

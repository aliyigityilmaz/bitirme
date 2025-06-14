using UnityEngine;

public class SceneSpawnPoint : MonoBehaviour
{
    public string spawnID; // örnek: "FromForest", "FromCave"

    void Start()
    {
        if (SceneTransitionManager.Instance == null)
            return;

        string targetID = SceneTransitionManager.Instance.GetSpawnPoint();
        if (spawnID == targetID)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = transform.position;
        }
    }
}

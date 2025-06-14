using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string spawnPointIDInTargetScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.Instance.SetSpawnPoint(spawnPointIDInTargetScene);
            SceneManager.LoadScene(targetSceneName);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenWorldCombatDataConnection : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "OpenWorldScene")
        {
            foreach (var ht in FindObjectsOfType<HeroTargetable>())
            {
                ht.LoadState();
                if (ht.heroData.health <= 0)
                    ht.gameObject.SetActive(false);
            }
        }
        else if (scene.name == "BCombatScene")
        {
            foreach (var ht in FindObjectsOfType<HeroTargetable>())
                ht.LoadState();
        }
    }
}
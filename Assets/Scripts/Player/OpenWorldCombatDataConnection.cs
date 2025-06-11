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
        if (scene.name == "OpenWorldScene" || scene.name == "BCombatScene")
        {
            foreach (var ht in FindObjectsOfType<HeroTargetable>())
            {
                ht.LoadState();

                // Eğer canı 0 ise, OpenWorld’de gizle
                if (ht.heroData.health <= 0)
                    ht.gameObject.SetActive(false);
            }
        }
    }
}
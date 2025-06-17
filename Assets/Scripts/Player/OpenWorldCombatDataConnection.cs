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
        if (scene.name == "Open World Level" || scene.name == "BCombatScene")
        {
            foreach (var ht in FindObjectsOfType<HeroTargetable>())
            {
                ht.LoadState();

                // Eğer canı 0 ise, OpenWorld’de gizle
                if (ht.heroData.health <= 0)
                    ht.gameObject.SetActive(false);
            }
            // 👇 SADECE OpenWorld için level sonucu bilgisi
            if (scene.name == "Open World Level")
            {
                int level = EncounterLevelTracker.currentLevel;

                if (EncounterResultData.HasResult(level))
                {
                    bool won = EncounterResultData.GetResult(level);
                    Debug.Log($"Open World Level’e dönüldü → Level {level} sonucu: {(won ? "KAZANDI" : "KAYBETTİ")}");
                }
                else
                {
                    Debug.Log("Open World Level’e dönüldü ama level sonucu bulunamadı.");
                }
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    private List<Enemy> allEnemies = new List<Enemy>();
    private List<RespawnData> respawnQueue = new List<RespawnData>();
    private HashSet<string> deadEnemies = new HashSet<string>();
    private HashSet<int> defeatedLevels = new HashSet<int>();

    public bool IsLevelDefeated(int level)
    {
        return defeatedLevels.Contains(level);
    }

    public void MarkLevelAsDefeated(int level)
    {
        if (!defeatedLevels.Contains(level))
            defeatedLevels.Add(level);
    }

    public void RegisterDeadEnemy(string enemyID)
    {
        if (!deadEnemies.Contains(enemyID))
        {
            deadEnemies.Add(enemyID);
            SaveDeadEnemies();
        }
    }

    public bool IsEnemyDead(string enemyID)
    {
        return deadEnemies.Contains(enemyID);
    }

    private void SaveDeadEnemies()
    {
        string data = string.Join(",", deadEnemies);
        PlayerPrefs.SetString("DeadEnemies", data);
    }

    private void LoadDeadEnemies()
    {
        deadEnemies.Clear();
        if (PlayerPrefs.HasKey("DeadEnemies"))
        {
            string data = PlayerPrefs.GetString("DeadEnemies");
            deadEnemies = new HashSet<string>(data.Split(','));
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // sahneler arası kalıcı

        LoadDeadEnemies(); // <- ÖNEMLİ!
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!allEnemies.Contains(enemy))
            allEnemies.Add(enemy);
    }

    public void RegisterForRespawn(Enemy enemy, float respawnAtTime)
    {
        respawnQueue.Add(new RespawnData { enemy = enemy, respawnTime = respawnAtTime });
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "BCombatScene") return;
        float currentTime = DayNightManager.Instance.currentTime;
        float realtime = Time.time;

        // 1. Respawn zaman� gelmi� d��manlar� aktif et
        for (int i = respawnQueue.Count - 1; i >= 0; i--)
        {
            var data = respawnQueue[i];
            if (realtime >= data.respawnTime)
            {
                if (data.enemy.IsWithinSpawnTime(currentTime))
                {
                    data.enemy.Respawn();
                }
                else
                {
                    // Spawn zaman� de�ilse listeden silmeden bekletiyoruz
                    continue;
                }

                respawnQueue.RemoveAt(i);
            }
        }
        // 2. Zaman aral��� d���nda olan d��manlar� deaktif et
        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue; // Ekledik

            // Zaten ölü ve yeniden doğma zamanı bekliyorsa skip
            if (IsEnemyDead(enemy.enemyID) && !respawnQueue.Exists(e => e.enemy == enemy))
            {
                if (enemy != null && enemy.gameObject != null) // 💡 Güvenli kontrol
                    enemy.gameObject.SetActive(false);
                continue;
            }

            if (enemy.useTimeRestrictions)
            {
                bool shouldBeActive = enemy.IsWithinSpawnTime(currentTime);

                if (shouldBeActive && !enemy.gameObject.activeSelf && !respawnQueue.Exists(e => e.enemy == enemy))
                {
                    enemy.Respawn();
                }
                else if (!shouldBeActive && enemy.gameObject.activeSelf)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }


    }

    private class RespawnData
    {
        public Enemy enemy;
        public float respawnTime;
    }
}

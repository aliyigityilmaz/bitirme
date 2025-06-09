using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    private List<Enemy> allEnemies = new List<Enemy>();
    private List<RespawnData> respawnQueue = new List<RespawnData>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
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
        float currentTime = DayNightManager.Instance.currentTime;
        float realtime = Time.time;

        // 1. Respawn zamaný gelmiþ düþmanlarý aktif et
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
                    // Spawn zamaný deðilse listeden silmeden bekletiyoruz
                    continue;
                }

                respawnQueue.RemoveAt(i);
            }
        }

        // 2. Zaman aralýðý dýþýnda olan düþmanlarý deaktif et
        foreach (var enemy in allEnemies)
        {
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

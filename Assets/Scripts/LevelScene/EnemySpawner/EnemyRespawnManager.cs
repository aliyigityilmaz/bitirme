using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    public static EnemyRespawnManager Instance;

    private class RespawnData
    {
        public Enemy enemy;
        public float respawnAt;
    }

    private List<RespawnData> respawnList = new List<RespawnData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        float currentTime = Time.time;
        for (int i = respawnList.Count - 1; i >= 0; i--)
        {
            if (currentTime >= respawnList[i].respawnAt)
            {
                respawnList[i].enemy.Respawn(); // Yeni fonksiyon çaðrýlýyor
                respawnList.RemoveAt(i);
            }
        }
    }

    public void RegisterForRespawn(Enemy enemy, float respawnAt)
    {
        respawnList.Add(new RespawnData
        {
            enemy = enemy,
            respawnAt = respawnAt
        });
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Inspector’da kullanacağın yardımcı sınıflar
[System.Serializable]
public class PrefabCount
{
    public GameObject prefab;
    public int       count;
}

[System.Serializable]
public class LevelSpawnConfig
{
    public int               level;
    public List<PrefabCount> spawns = new List<PrefabCount>();
}

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance;

    [Header("Level Bazlı Spawn Ayarları")]
    [SerializeField]
    private List<LevelSpawnConfig> levelConfigs = new List<LevelSpawnConfig>();

    // Sahne yüklendikten sonra burası otomatik dolacak
    private Transform[] spawnPoints;

    // Planlanan prefab ve adet bilgisi
    private Dictionary<GameObject,int> prefabSpawnPlan = new Dictionary<GameObject,int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Overworld’dan Combat’a geçmeden önce çağıracağın metot
    public void SetupEncounterForLevel(int level)
    {
        prefabSpawnPlan.Clear();

        // Inspector’daki config listesinden seviye eşleşmesini al
        var cfg = levelConfigs.FirstOrDefault(c => c.level == level);
        if (cfg == null)
        {
            Debug.LogWarning($"[EncounterManager] Level {level} için config bulunamadı!");
            return;
        }

        // Her PrefabCount için plana ekle
        foreach (var pc in cfg.spawns)
        {
            if (pc.prefab != null && pc.count > 0)
                prefabSpawnPlan[pc.prefab] = pc.count;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex !=1 ) return;

        // 1) Tag ile Combat sahnesindeki SpawnPoint objelerini bul
        spawnPoints = GameObject
            .FindGameObjectsWithTag("SpawnPoint")
            .OrderBy(go => go.name)      // İsim sırasına göre; opsiyonel
            .Select(go => go.transform)
            .ToArray();

        // 2) Planlanan enemy’leri instantiate et
        SpawnEnemiesInCombat();
    }

    private void SpawnEnemiesInCombat()
    {
        int idx = 0;
        foreach (var kv in prefabSpawnPlan)
        {
            var prefab = kv.Key;
            int  cnt    = kv.Value;

            for (int i = 0; i < cnt && idx < spawnPoints.Length; i++)
            {
                Instantiate(prefab,
                            spawnPoints[idx].position,
                            spawnPoints[idx].rotation);
                idx++;
            }
        }

        prefabSpawnPlan.Clear();
    }
}

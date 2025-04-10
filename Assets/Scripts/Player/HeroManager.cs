using UnityEngine; using UnityEngine.SceneManagement; using System.Collections.Generic;

public class HeroManager : MonoBehaviour
{ 
    public static HeroManager instance;
    public List<Hero> heroList = new List<Hero>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // Sahne yüklendiğinde listemizi güncellemek için event'e abone oluyoruz
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Bellek sızıntısını önlemek için event aboneliğini kaldırıyoruz
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

// Yeni bir sahne yüklendiğinde çağrılır
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateHeroList();
    }

// Sahnedeki tüm HeroTargetable ve EnemyTargetable bileşenlerini bularak heroList'e ekler
    private void PopulateHeroList()
    {
        // Önce listede önceki sahneden kalan verileri temizleyin
        heroList.Clear();

        // Sahnedeki tüm HeroTargetable nesnelerinden verileri al
        HeroTargetable[] heroTargets = GameObject.FindObjectsOfType<HeroTargetable>();
        foreach (var ht in heroTargets)
        {
            if (ht.heroData != null)
            {
                heroList.Add(ht.heroData);
            }
        }

        // Sahnedeki tüm EnemyTargetable nesnelerinden verileri al
        EnemyTargetable[] enemyTargets = GameObject.FindObjectsOfType<EnemyTargetable>();
        foreach (var et in enemyTargets)
        {
            if (et.enemyData != null)
            {
                heroList.Add(et.enemyData);
            }
        }
    }
}
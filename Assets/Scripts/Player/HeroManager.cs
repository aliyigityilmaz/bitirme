using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateHeroList();
    }

    private void PopulateHeroList()
    {
        heroList.Clear();

        foreach (var ht in FindObjectsOfType<HeroTargetable>())
        {
            if (ht.heroData != null && ht.gameObject.activeInHierarchy)
            {
                heroList.Add(ht.heroData);
            }
        }

        foreach (var et in FindObjectsOfType<EnemyTargetable>())
        {
            if (et.enemyData != null && et.gameObject.activeInHierarchy)
            {
                heroList.Add(et.enemyData);
            }
        }

        Debug.Log($"[HeroManager] Sahnede bulunan toplam aktif karakter: {heroList.Count}");
    }
}
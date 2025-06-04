// OpenWorldCombatDataConnection.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        // Sahne adlarına göre karar verelim:
        if (scene.name == "CombatScene") 
        {
            // Combat sahnesi yükleniyor: Open World'den gelen verileri yükle
            // CombatStateManager.Start() içinde turnOrder oluşturulurken
            // HeroSaveManager.LoadHeroes, HeroManager.instance.heroList'i günceller.
            HeroSaveManager.LoadHeroes(HeroManager.instance.heroList);
            // CombatStateManager’in Start() içinde turnOrder = HeroManager... sıralaması mevcut
        }
        else if (scene.name == "OpenWorldScene") 
        {
            // Open World sahnesi başladı: Combat'tan gelen kayıtlı verileri yükle
            HeroSaveManager.LoadHeroes(HeroManager.instance.heroList);
            // Ölü kahramanları sahnede pasif hale getirebilirsiniz:
            foreach (var hero in HeroManager.instance.heroList)
            {
                if (hero.health <= 0)
                {
                    // Ölü kahraman için açık dünyadaki objeyi devre dışı bırakın:
                    var ht = FindHeroTargetableById(hero.id);
                    if (ht != null) 
                        ht.gameObject.SetActive(false);
                }
            }
        }
    }

    // ID’ye göre sahnedeki HeroTargetable'ı bulur:
    private HeroTargetable FindHeroTargetableById(int id)
    {
        foreach (var ht in GameObject.FindObjectsOfType<HeroTargetable>())
        {
            if (ht.heroData != null && ht.heroData.id == id)
                return ht;
        }
        return null;
    }
}

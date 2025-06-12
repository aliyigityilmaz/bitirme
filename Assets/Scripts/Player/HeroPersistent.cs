using System.Collections.Generic;
using UnityEngine;

public class HeroPersistent : MonoBehaviour
{
    private Dictionary<int, HeroData> heroDataMap = new Dictionary<int, HeroData>();

    private static HeroPersistent _instance;
    public static HeroPersistent instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject(nameof(HeroPersistent));
                _instance = go.AddComponent<HeroPersistent>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    public void UpdateHeroData(Hero hero)
    {
        heroDataMap[hero.id] = hero.ToHeroData();
    }

    public HeroData GetHeroDataById(int id)
    {
        heroDataMap.TryGetValue(id, out var data);
        return data;
    }
    
   public void UpdateHealth(int id,int health)
    {
        if (heroDataMap.TryGetValue(id, out var heroData))
        {
            heroData.health += health;
            heroDataMap[id] = heroData; // Güncellenmiş veriyi tekrar kaydet
        }
        else
        {
            Debug.LogWarning($"Hero ID {id} bulunamadı.");
        }
    }

    public void UpdateArmor(int id, int armor)
    {
        if (heroDataMap.TryGetValue(id, out var heroData))
        {
            heroData.armor += armor;
            heroDataMap[id] = heroData; // Güncellenmiş veriyi tekrar kaydet
        }
        else
        {
            Debug.LogWarning($"Hero ID {id} bulunamadı.");
        }
    }
    public void UpdateCriticalChance(int id, int criticalChance)
    {
        if (heroDataMap.TryGetValue(id, out var heroData))
        {
            heroData.criticalChance += criticalChance;
            heroDataMap[id] = heroData; // Güncellenmiş veriyi tekrar kaydet
        }
        else
        {
            Debug.LogWarning($"Hero ID {id} bulunamadı.");
        }
    }
    public void UpdateTurnSpeed(int id, int turnSpeed)
    {
        if (heroDataMap.TryGetValue(id, out var heroData))
        {
            heroData.turnSpeed += turnSpeed;
            heroDataMap[id] = heroData; // Güncellenmiş veriyi tekrar kaydet
        }
        else
        {
            Debug.LogWarning($"Hero ID {id} bulunamadı.");
        }
    }
}

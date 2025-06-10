using System.Collections.Generic;
using UnityEngine;

public class HeroPersistent : MonoBehaviour
{
    
    private Dictionary<int, HeroSaveData> heroDataMap = new Dictionary<int, HeroSaveData>();
    
    
    
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


    public void UpdateHeroData(HeroSaveData data)
    {
        heroDataMap[data.id] = data;
    }

    public HeroSaveData GetHeroById(int id)
    {
        heroDataMap.TryGetValue(id, out var data);
        return data;
    }
}
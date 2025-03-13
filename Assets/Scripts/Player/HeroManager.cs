using System;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else
        {
                Destroy(gameObject);
        }
    }
    
    
}

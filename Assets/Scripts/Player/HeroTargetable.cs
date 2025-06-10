using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroTargetable : MonoBehaviour
{
    public Hero heroData;
    private Collider myCollider;
    

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        
        
    }

    private void Start()
    {
        LoadState();
        if (heroData.health <= 0)
            myCollider.enabled = false;
    }


    public void SaveState()
    {
        var save = new HeroSaveData(heroData);
        HeroPersistent.instance.UpdateHeroData(save);
        Debug.Log("Hero state saved: " + heroData.id);
    }

    public void LoadState()
    {
        if (HeroPersistent.instance == null)
        {
            Debug.LogError("HeroPersistentData.instance’da değer yok!");
            Debug.Log("Loading state for hero " + heroData.id);
            return;
        }
        if (heroData == null)
        {
            Debug.LogError("heroData atanmamış!");
            return;
        }

        var pd = HeroPersistent.instance.GetHeroById(heroData.id);
        if (pd == null)
            return;

        heroData.health         = pd.health;
        heroData.baseHealth     = pd.baseHealth;
        heroData.armor          = pd.armor;
        heroData.turnSpeed      = pd.turnSpeed;
        heroData.criticalChance = pd.criticalChance;
    }
    public void Die()
    {
        heroData.health = 0;
        SaveState();
        myCollider.enabled = false;
        heroData.charAnimator.SetTrigger("Death");
        CombatStateManager.Instance.RemoveFromTurnOrder(heroData);
        CombatStateManager.Instance.CheckBattleEnd();
    }
}
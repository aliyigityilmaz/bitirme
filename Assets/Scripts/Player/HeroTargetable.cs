#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

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
        heroData.outline.enabled = false;
        
        LoadState();

        if (heroData.health <= 0)
            myCollider.enabled = false;

        heroData.UpdateHealthBar();
    }

    public void SaveState()
    {
        HeroPersistent.instance.UpdateHeroData(heroData);
        Debug.Log("Hero state saved: " + heroData.id);
    }

    public void LoadState()
    {
        var pd = HeroPersistent.instance.GetHeroDataById(heroData.id);
        if (pd != null)
        {
            heroData.LoadFromHeroData(pd);
        }
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

    private void OnMouseDown()
    {
        if (!CombatStateManager.Instance.IsTargetSelectionActive) return;

        Skill selectedSkill = CombatStateManager.Instance.selectedSkill;

        if (selectedSkill == null) return;

        // Heal ise kendi takımına
        if (selectedSkill.skillType == SkillType.Heal && heroData.team == TeamType.Hero)
        {
            CombatStateManager.Instance.OnHeroSelected(heroData);
            SkillUIManager.Instance.skillPanel.SetActive(false);
        }
    }
    

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying) return;

        // Inspector’da canlı güncelleme için
        EditorUtility.SetDirty(this);
    }
#endif
}
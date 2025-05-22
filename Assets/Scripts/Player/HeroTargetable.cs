using UnityEngine;

public class HeroTargetable : MonoBehaviour
{
    public Hero heroData;
    private Collider myCollider;
    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }
    private void OnMouseDown()
    {
        if (CombatStateManager.Instance.IsTargetSelectionActive)
        {
            Skill selectedSkill = CombatStateManager.Instance.selectedSkill;

            if (selectedSkill != null && selectedSkill.skillType == SkillType.Heal)
            {
                CombatStateManager.Instance.OnHeroSelected(heroData);
                SkillUIManager.Instance.skillPanel.SetActive(false);
            }
        }
    }
    public void Die()
    {
        Debug.Log($"{heroData.name} died.");

        // 1. HeroManager listesinden çýkar
        HeroManager.instance.heroList.Remove(heroData);

        CombatStateManager.Instance.RemoveFromTurnOrder(heroData);

        if (myCollider != null)
            myCollider.enabled = false;

        gameObject.SetActive(false);

        CombatStateManager.Instance.CheckBattleEnd();
    }
}


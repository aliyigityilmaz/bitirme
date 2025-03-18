using UnityEngine;

public class HeroTargetable : MonoBehaviour
{
    public Hero heroData;

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
}

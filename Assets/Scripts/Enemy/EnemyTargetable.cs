using UnityEngine;

public class EnemyTargetable : MonoBehaviour
{
    public Hero enemyData;

    private void OnMouseDown()
    {
        if (CombatStateManager.Instance.IsTargetSelectionActive)
        {
            Skill selectedSkill = CombatStateManager.Instance.selectedSkill;

            if (selectedSkill != null && selectedSkill.skillType == SkillType.Damage)
            {
                CombatStateManager.Instance.OnEnemySelected(enemyData);
                SkillUIManager.Instance.skillPanel.SetActive(false);
            }
        }
    }
}

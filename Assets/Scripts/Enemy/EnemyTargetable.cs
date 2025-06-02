using UnityEngine;

public class EnemyTargetable : MonoBehaviour
{
    public Hero enemyData;
    private Collider myCollider;
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
    public void Die()
    {
        Debug.Log($"{enemyData.name} died.");

        HeroManager.instance.heroList.Remove(enemyData);

        CombatStateManager.Instance.RemoveFromTurnOrder(enemyData);

        if (myCollider != null)
            myCollider.enabled = false;

        gameObject.SetActive(false);

        CombatStateManager.Instance.CheckBattleEnd();
    }
}

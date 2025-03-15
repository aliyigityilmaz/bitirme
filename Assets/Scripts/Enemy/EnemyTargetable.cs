using UnityEngine;

public class EnemyTargetable : MonoBehaviour
{
    public Hero enemyData;

    private void OnMouseDown()
    {
        if (CombatStateManager.Instance.IsTargetSelectionActive)
        {
            CombatStateManager.Instance.OnEnemySelected(enemyData);
            SkillUIManager.Instance.skillPanel.SetActive(false);
        }
    }
}

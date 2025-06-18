using UnityEngine;

public class EnemyTargetable : MonoBehaviour
{
    public Hero enemyData;
    private Collider myCollider;

    public Transform assignedSpawnPoint;
    void Start()
    {
        if (assignedSpawnPoint == null)
        {
            Debug.Log($"{gameObject.name} için spawn point atanmamýþ, en yakýn spawn point aranýyor...");
            assignedSpawnPoint = FindNearestSpawnPoint();

            if (assignedSpawnPoint == null)
            {
                Debug.LogWarning($"{gameObject.name} için en yakýn spawn point bulunamadý!");
            }
            else
            {
                Debug.Log($"{gameObject.name} için en yakýn spawn point bulundu: {assignedSpawnPoint.name}");
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} için spawn point manuel olarak atandý: {assignedSpawnPoint.name}");
        }
    }

    Transform FindNearestSpawnPoint()
    {
        if (VFXActivator.instance == null)
        {
            Debug.LogWarning("VFXActivator instance bulunamadý!");
            return null;
        }

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var sp in VFXActivator.instance.projectileSpawnPointsForEnemies)
        {
            float dist = Vector3.Distance(transform.position, sp.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = sp;
            }
        }

        return nearest;
    }
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
        enemyData.charAnimator.SetTrigger("Death");
       //gameObject.SetActive(false);
       
        CombatStateManager.Instance.CheckBattleEnd();
    }
}

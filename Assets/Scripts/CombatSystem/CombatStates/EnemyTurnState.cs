using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyTurnState : ICombatState
{
    private CombatStateManager manager;

    public EnemyTurnState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Entering Enemy Turn State");
        manager.StartCoroutine(DelayedEnemyActionStart());
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Exiting Enemy Turn State");
    }
    private IEnumerator DelayedEnemyActionStart()
    {
        yield return new WaitForSeconds(2f);
        yield return manager.StartCoroutine(PerformEnemyAction());
    }
    private IEnumerator PerformEnemyAction()
    {
        Hero enemyHero = manager.turnOrder[manager.currentTurnIndex];

        if (enemyHero.skills == null || enemyHero.skills.Length == 0)
        {
            Debug.Log("Enemy " + enemyHero.name + " has no skills. Ending turn.");
            EndTurn();
            yield break;
        }

        int randomSkillIndex = Random.Range(0, enemyHero.skills.Length);
        Skill randomSkill = enemyHero.skills[randomSkillIndex];

        List<Hero> potentialTargets = HeroManager.instance.heroList
            .Where(h => h.team == TeamType.Hero)
            .ToList();

        if (potentialTargets.Count == 0)
        {
            Debug.Log("No hero targets available. Ending turn.");
            EndTurn();
            yield break;
        }

        int randomTargetIndex = Random.Range(0, potentialTargets.Count);
        Hero targetHero = potentialTargets[randomTargetIndex];
        EnemyTargetable enemyTargetable = Object.FindObjectsOfType<EnemyTargetable>()
            .FirstOrDefault(ht => ht.enemyData == enemyHero);

        bool isBoss = enemyTargetable != null && enemyTargetable.CompareTag("Boss");

        string triggerName = isBoss ? randomSkill.skillName : "BasicAttack";

        enemyHero.charAnimator.SetTrigger(triggerName);


        Debug.Log($"{enemyHero.name} uses {randomSkill.skillName} on {targetHero.name}");

        yield return new WaitForSeconds(2f);

        int damage = randomSkill.baseDamage;
        targetHero.health -= damage;
        targetHero.heroHitVFX.Play();
        targetHero.charAnimator.SetTrigger("TakeDamage");
        if (targetHero.health <= 0)
        {
            var allHeroTargets = Object.FindObjectsOfType<HeroTargetable>();
            foreach (var ht in allHeroTargets)
            {
                if (ht.heroData == targetHero)
                {
                    ht.Die();
                    break;
                }
            }
        }
        Debug.Log($"{targetHero.name}'s health is now {targetHero.health} after taking {damage} damage.");
        EndTurn();
    }
    private void EndTurn()
    {
        manager.NextTurn();
        manager.SetState(new IdleState(manager));
    }
}

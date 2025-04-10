using UnityEngine;
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
        PerformEnemyAction();
    }

    public void Execute()
    {
       
    }

    public void Exit()
    {
        Debug.Log("Exiting Enemy Turn State");
    }

    private void PerformEnemyAction()
    {
        Hero enemyHero = manager.turnOrder[manager.currentTurnIndex];
        //rastgele skill seçimi
        int randomSkillIndex = Random.Range(0, enemyHero.skills.Length);
        Skill randomSkill = enemyHero.skills[randomSkillIndex];

        List<Hero> potentialTargets = HeroManager.instance.heroList.Where(h => h.team == TeamType.Hero).ToList();

        if (potentialTargets.Count == 0)
        {
            Debug.Log("No hero targets available.");
            EndTurn();
            return;
        }
        //rastgele hero seçimi
        int randomTargetIndex = Random.Range(0, potentialTargets.Count);
        Hero targetHero = potentialTargets[randomTargetIndex];
        Debug.Log($"{enemyHero.name} uses {randomSkill.skillName} on {targetHero.name}");

        EndTurn();
    }

    private void EndTurn()
    {
        manager.NextTurn();
        manager.SetState(new IdleState(manager));
    }
}

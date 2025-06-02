using UnityEngine;
using System.Collections;

public class PlayerTurnState : ICombatState
{
    private CombatStateManager manager;

    public PlayerTurnState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        manager.StartCoroutine(DelayedEnter());
    }

    private IEnumerator DelayedEnter()
    {
        yield return new WaitForSeconds(2f);

        SkillUIManager.Instance.skillPanel.SetActive(true);
        Debug.Log("Entering Player Turn State");
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        SkillUIManager.Instance.InitializeSkills(activeHero);
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.SetState(new PlayerInputState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Turn State");
        //SkillUIManager.Instance.skillPanel.SetActive(false);
    }
}

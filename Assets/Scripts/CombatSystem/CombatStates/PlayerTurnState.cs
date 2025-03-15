using UnityEngine;

public class PlayerTurnState : ICombatState
{
    private CombatStateManager manager;
//skill secimi
    public PlayerTurnState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        SkillUIManager.Instance.skillPanel.SetActive(true);
        Debug.Log("Entering Player Turn State");
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        SkillUIManager.Instance.InitializeSkills(activeHero);
    }

    public void Execute()
    {//skil secme bura
        // Örnek: Oyuncunun hazır olduğu varsayılarak (örneğin P tuşu) input state'e geçiş.
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
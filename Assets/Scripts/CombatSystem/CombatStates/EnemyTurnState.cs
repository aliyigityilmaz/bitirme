using UnityEngine;

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
        // Düşmanın aksiyonlarını başlatabilirsiniz.
    }

    public void Execute()
    {
        // Örnek: E tuşuna basıldığında enemy turn tamamlanıp Idle state'e dönüş.
        if (Input.GetKeyDown(KeyCode.Space))
        {manager.NextTurn();
            manager.SetState(new IdleState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Enemy Turn State");
    }
}
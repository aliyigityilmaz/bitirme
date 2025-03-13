using UnityEngine;

public class PlayerActionState : ICombatState
{
    private CombatStateManager manager;
    
    //skil vurması
    public PlayerActionState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {// damage hesaplama
        Debug.Log("Entering Player Action State");
        // Burada skill, animasyon veya saldırı gibi işlemleri tetikleyebilirsiniz.
    }

    public void Execute()
    {
        // Örnek: A tuşuna basıldığında aksiyon tamamlanıp enemy turn state'e geçelim.
        if (Input.GetKeyDown(KeyCode.Space))
        {   manager.NextTurn();
            manager.SetState(new IdleState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Action State");
    }
}
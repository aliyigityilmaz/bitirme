using UnityEngine;

public class PlayerInputState : ICombatState
{
    private CombatStateManager manager;
//skil damagae hesabı mevuzu
    public PlayerInputState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Entering Player Input State");
    }

    public void Execute()
    {
        // Örnek: Girdi alındığında (örneğin I tuşu) oyuncu aksiyonuna geçilsin.
        if (Input.GetKeyDown(KeyCode.I))
        {
            manager.SetState(new PlayerActionState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Input State");
    }
}
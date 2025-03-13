using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : ICombatState
{
    private CombatStateManager manager;
    private List<Hero> turnList;
    private int currentTurn;
    
//hangi karaktere geçiliceği burda tutulucak hesap baska sc 
//her karakter vurdugunda tur sayıcak tur hesabı burda tutuulucak
    public IdleState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    { 

    }

    public void Execute()
    {
        // Örnek geçiş koşulu: Space tuşuna basıldığında oyuncu sırasına geçilsin.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.SetState(new PlayerTurnState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
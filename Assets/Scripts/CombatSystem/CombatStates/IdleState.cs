using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : ICombatState
{
    private CombatStateManager manager;
    private int currentTurn;
    
//hangi karaktere geçiliceği burda tutulucak hesap baska sc 
//her karakter vurdugunda tur sayıcak tur hesabı burda tutuulucak
    public IdleState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        // Sıradaki karakteri alıyoruz
        Hero currentHero = manager.turnOrder[manager.currentTurnIndex];

        Debug.Log($"Idle State: Sıradaki karakter = {currentHero.name}, " +
                 $"ID = {currentHero.id}, Team = {currentHero.team}");
        // heroEnemy alanına göre hangi state'e geçeceğimize karar veriyoruz
        if (currentHero.team == TeamType.Hero)
        {
            Debug.Log("Karakter Hero. PlayerTurnState'e geçiliyor.");
            manager.SetState(new PlayerTurnState(manager));
        }
        else
        {
            Debug.Log("Karakter Enemy. EnemyTurnState'e geçiliyor.");
            manager.SetState(new EnemyTurnState(manager));
        }


    }

    public void Execute()
    {

    }
    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}
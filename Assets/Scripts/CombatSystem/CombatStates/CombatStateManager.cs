using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatStateManager : MonoBehaviour
{
    [SerializeField] private string currentStateName;

    private ICombatState currentState;
    
    public List<Hero> turnOrder = new List<Hero>();
    public int currentTurnIndex = 0;

    private void Start()
    {
        // Başlangıçta Idle state ile başlıyoruz
        turnOrder = HeroManager.instance.heroList
            .OrderByDescending(h => h.turnSpeed) // turnSpeed’e göre sıralama
            .ToList();
        SetState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Execute();
    }

    public void SetState(ICombatState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
        // Aktif state'in ismini güncelliyoruz
        currentStateName = currentState.GetType().Name;
        Debug.Log("Combat State changed to: " + currentStateName);
    }
    public void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        Debug.Log("Next turn index is now " + currentTurnIndex);
    }
}
using UnityEngine;

public class CombatStateManager : MonoBehaviour
{
    [SerializeField] private string currentStateName;

    private ICombatState currentState;

    private void Start()
    {
        // Başlangıçta Idle state ile başlıyoruz
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
}
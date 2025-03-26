using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class CombatStateManager : MonoBehaviour
{
    public static CombatStateManager Instance;

    [SerializeField] private string currentStateName;

    private ICombatState currentState;
    
    public List<Hero> turnOrder = new List<Hero>();
    public int currentTurnIndex = 0;

    public int selectedSkillIndex = -1;
    public bool IsTargetSelectionActive = false;

    //skill ve enemy bilgisini saklamak için
    public Skill selectedSkill;
    public Hero selectedEnemy;


    private void Start()
    {
        Instance = this;
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
    public void OnEnemySelected(Hero enemy)
    {
        if (selectedSkillIndex != -1)
        {
            Hero activeHero = turnOrder[currentTurnIndex];
            selectedSkill = activeHero.GetSkills()[selectedSkillIndex];
            Debug.Log("Selected skill: " + selectedSkill.skillName);
            Debug.Log("Selected enemy: " + enemy.name);
            selectedEnemy = enemy;
            IsTargetSelectionActive = false;
            SetState(new PlayerInputState(this));
        }
    }
    public void OnHeroSelected(Hero hero)
    {
        if (selectedSkillIndex != -1)
        {
            Hero activeHero = turnOrder[currentTurnIndex];
            selectedSkill = activeHero.GetSkills()[selectedSkillIndex];
            selectedEnemy = hero;
            IsTargetSelectionActive = false;

            Debug.Log($"Target Selected: {selectedEnemy.name}");

            SetState(new PlayerInputState(this));
        }
    }
}
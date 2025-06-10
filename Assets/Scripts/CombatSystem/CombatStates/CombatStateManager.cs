using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    [Header("End Screen")]
    public GameObject winPanel;
    public GameObject losePanel;

    

    private void Start()
    {
        Instance = this;
        // Başlangıçta Idle state ile başlıyoruz
        turnOrder = HeroManager.instance.heroList
            .Where(h => h.health > 0)
            .OrderByDescending(h => h.turnSpeed) // turnSpeed’e göre sıralama
            .ToList();
        SetState(new IdleState(this));
    }

    private void Update()
    {
        currentState?.Execute();
        if (currentTurnIndex >= turnOrder.Count || currentTurnIndex < 0)
        {
            Debug.LogError($"currentTurnIndex: {currentTurnIndex}, turnOrder.Count: {turnOrder.Count}");
            return;
        }
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
    public void StartTurn()
    {
        //hero cooldown için
        Hero activeHero = turnOrder[currentTurnIndex];

        foreach (var skill in activeHero.GetSkills())
        {
            skill.ReduceCooldown();
        }

    }
    public void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        StartTurn();
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
    // Add this method to CombatStateManager
   
    public void OnHeroSelected(Hero hero)
    {
        if (selectedSkillIndex != -1)
        {
            Hero activeHero = turnOrder[currentTurnIndex];
            selectedSkill = activeHero.GetSkills()[selectedSkillIndex];
            selectedEnemy = hero;
            IsTargetSelectionActive = false;

            Debug.Log($"Target Selected: {selectedEnemy.name}");
            selectedEnemy.charAnimator.SetTrigger("TakeDamage");
            SetState(new PlayerInputState(this));
        }
    }
    public void EndBattle(bool isWin)
    {
        // Combat bitmeden önce kahraman verilerini kaydet:
        HeroSaveManager.SaveHeroes(HeroManager.instance.heroList);
        
    
        SetState(new EndBattleState(this));

        if (isWin)
            winPanel.SetActive(true);
        else
            losePanel.SetActive(true);


    }
    public void CheckBattleEnd()
    {
        bool heroesAlive = HeroManager.instance.heroList.Any(h => h.team == TeamType.Hero && h.health > 0);
        bool enemiesAlive = HeroManager.instance.heroList.Any(h => h.team == TeamType.Enemy && h.health > 0);

        if (!heroesAlive)
        {
            Debug.Log("Tüm kahramanlar öldü");
            EndBattle(false);
        }
        else if (!enemiesAlive)
        {
            Debug.Log("Tüm düşmanlar öldü");
            EndBattle(true);
        }
    }
    public void RemoveFromTurnOrder(Hero h)
    {
        int removedIndex = turnOrder.IndexOf(h);
        if (removedIndex == -1) return;

        turnOrder.RemoveAt(removedIndex);

        if (removedIndex < currentTurnIndex)
        {
            currentTurnIndex--;
        }
        if (turnOrder.Count == 0)
        {
            currentTurnIndex = 0;
        }
        else
        {
            currentTurnIndex = currentTurnIndex % turnOrder.Count;
        }
    }
}
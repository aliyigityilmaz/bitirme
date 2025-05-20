using UnityEngine;

public class PlayerActionState : ICombatState
{
    private CombatStateManager manager;
    private float damageTimer = 2f;
    private bool isAnimStarted = false;
    private CombatCameraManager cameraManager;
    //skil vurması
    public PlayerActionState(CombatStateManager manager)
    {
        this.manager = manager;
    }
    
    public void Enter()
    {// damage hesaplama
        Debug.Log("Entering Player Action State");
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        activeHero.charAnimator.SetTrigger("BasicAttack");
        isAnimStarted = true;
        damageTimer = 2f;
    }

    public void Execute()
    {
        // Örnek: A tuşuna basıldığında aksiyon tamamlanıp enemy turn state'e geçelim.
        if (Input.GetKeyDown(KeyCode.Space))
        {   manager.NextTurn();
            manager.SetState(new IdleState(manager));
        }
        if (isAnimStarted)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0)
            {
                ApplySkill(PlayerInputState.instance.finalDamage); 
                damageTimer = 2f; 
                isAnimStarted = false; 
            }
        }
        
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Action State");
        CombatCameraManager.instance.SetCameraPosition(CombatCameraManager.instance. mainCameraTransform);
    }

    public void ApplySkill(float finalMultiplier)
    {
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        Skill selectedSkill = activeHero.GetSkills()[manager.selectedSkillIndex];
        manager.selectedSkill = selectedSkill;
        float finalValue = selectedSkill.baseDamage * finalMultiplier;

        if (selectedSkill.skillType == SkillType.Damage)
        {
            if (manager.selectedEnemy != null)
            {
                manager.selectedEnemy.health -= (int)finalValue;
                Debug.Log($"{activeHero.name} {manager.selectedEnemy.name} üzerinde {(int)finalValue} hasar yaptı.");
                manager.NextTurn();
                manager.SetState(new IdleState(manager));
            }
        }
        else if (selectedSkill.skillType == SkillType.Heal)
        {
            if (manager.selectedEnemy != null)
            {
                manager.selectedEnemy.health += (int)finalValue;
                Debug.Log($"{activeHero.name} {manager.selectedEnemy.name} üzerinde {(int)finalValue} iyileştirme yaptı.");
                manager.NextTurn();
                manager.SetState(new IdleState(manager));
            }
        }
    }
}
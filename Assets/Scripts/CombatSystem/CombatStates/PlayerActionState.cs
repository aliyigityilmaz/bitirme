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
        Skill selectedSkill = activeHero.GetSkills()[manager.selectedSkillIndex];
        float multiplier = PlayerInputState.instance.finalDamage;
        if (activeHero.id == 1)
        {
            if (selectedSkill.skillId == SkillId.NormalAttack)
            {
                activeHero.charAnimator.SetTrigger("NormalAttack");
            }
            else if (selectedSkill.skillId == SkillId.SpecialAttack)
            {
                activeHero.charAnimator.SetTrigger("SpecialAttack1");
            }
            else if (selectedSkill.skillId == SkillId.Ulti)
            {
                if (multiplier >= 1.4f)
                {
                    activeHero.charAnimator.SetTrigger("Ulti3");
                }

                else if (multiplier >= 0.9f)
                {
                    activeHero.charAnimator.SetTrigger("Ulti2");
                }
                else
                {
                    activeHero.charAnimator.SetTrigger("Ulti1");
                }
            }
            else if (selectedSkill.skillId == SkillId.Max)
            {
                activeHero.charAnimator.SetTrigger("Max");
            }
        }
        else if (activeHero.id == 2)
        {
            if (selectedSkill.skillId == SkillId.NormalAttack)
            {
                activeHero.charAnimator.SetTrigger("NormalAttack");
            }
            else if (selectedSkill.skillId == SkillId.SpecialAttack)
            {
                activeHero.charAnimator.SetTrigger("SpecialAttack1");
            }
            else if (selectedSkill.skillId == SkillId.Ulti)
            {
                if (multiplier >= 1.4f)
                    activeHero.charAnimator.SetTrigger("Ulti1_3");
                else if (multiplier >= 0.9f)
                    activeHero.charAnimator.SetTrigger("Ulti1_2");
                else
                    activeHero.charAnimator.SetTrigger("Ulti1_1");
            }
        }
        else if (activeHero.id == 3)
        {
            if (selectedSkill.skillId == SkillId.NormalAttack)
            {
                activeHero.charAnimator.SetTrigger("NormalAttack");
            }
            else if (selectedSkill.skillId == SkillId.SpecialAttack)
            {
                activeHero.charAnimator.SetTrigger("SpecialAttack1");
            }
            else if (selectedSkill.skillId == SkillId.Ulti)
            {
                activeHero.charAnimator.SetTrigger("Ulti1");
            }

        }
        else if (activeHero.id == 4)
        {
            if (selectedSkill.skillId == SkillId.NormalAttack)
            {
                activeHero.charAnimator.SetTrigger("NormalAttack");
            }
            else if (selectedSkill.skillId == SkillId.SpecialAttack)
            {
                activeHero.charAnimator.SetTrigger("SpecialAttack1");
            }
            else if (selectedSkill.skillId == SkillId.Ulti)
            {
                activeHero.charAnimator.SetTrigger("Ulti1");
            }
        }
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
        selectedSkill.Activate();
        manager.selectedSkill = selectedSkill;
        float finalValue = selectedSkill.baseDamage * finalMultiplier;

        if (selectedSkill.skillType == SkillType.Damage)
        {
            if (manager.selectedEnemy != null)
            {
                manager.selectedEnemy.health -= (int)finalValue;
                manager.selectedEnemy.charAnimator.SetTrigger("TakeDamage");

                if (manager.selectedEnemy.health <= 0)
                {
                    Debug.Log($"{manager.selectedEnemy.name} has died!");

                    EnemyTargetable[] allHeroTargets = GameObject.FindObjectsOfType<EnemyTargetable>();
                    foreach (var ht in allHeroTargets)
                    {
                        if (ht.enemyData == manager.selectedEnemy)
                        {
                            ht.Die();
                            break;
                        }
                    }
                }
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
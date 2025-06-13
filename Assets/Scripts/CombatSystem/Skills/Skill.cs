using UnityEngine;

public enum SkillType
{
    Damage,
    Heal
}
public enum SkillId
{
    NormalAttack,
    SpecialAttack,
    Ulti,
    Max
}
[System.Serializable]
public class Skill
{
    public string skillName;
    public SkillType skillType;
    public SkillId skillId;
    public int baseDamage;
    public int baseHeal;
    [Header("Cooldown")]
    public int cooldownTurns;
    public int currentCooldown = 0;
    public int maxCooldown = 3;
    [Header("Sprite Deðiþimi")]
    public Sprite skillIcon;
    public void Activate()
    {
        Debug.Log($"{skillName} activated!");
        StartCooldown();
        // Skill'e ait diðer iþlemler burada gerçekleþtirilebilir.
    }
    public bool IsAvailable()
    {
        return currentCooldown == 0;
    }

    public void StartCooldown()
    {
        currentCooldown = maxCooldown;
    }

    public void ReduceCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown--;
    }
}

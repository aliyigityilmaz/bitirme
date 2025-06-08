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
    Ulti
}
[System.Serializable]
public class Skill
{
    public string skillName;
    public SkillType skillType;
    public SkillId skillId;
    public int baseDamage;
    public int baseHeal;
    public void Activate()
    {
        Debug.Log($"{skillName} activated!");
        // Skill'e ait diðer iþlemler burada gerçekleþtirilebilir.
    }
}

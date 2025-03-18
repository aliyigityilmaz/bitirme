using UnityEngine;

public enum SkillType
{
    Damage,
    Heal
}
[System.Serializable]
public class Skill
{
    public string skillName;
    public SkillType skillType;
    public int baseDamage;
    public int baseHeal;

    public void Activate()
    {
        Debug.Log($"{skillName} activated!");
        // Skill'e ait diðer iþlemler burada gerçekleþtirilebilir.
    }
}

using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Hero,
    Enemy
}

[System.Serializable]
public class Hero
{
    public string name;
    public TeamType team;
    public int id;
    public int baseHealth;
    public int health;
    public int turnSpeed;
    public int armor;
    public int criticalChance;
    public Skill[] skills;
    public Animator charAnimator;
    public ParticleSystem heroHitVFX;
    public Transform heroTransform;
    public Skill[] GetSkills() { return skills; }

    public Hero(string name, int id, int health, int turnSpeed, int armor, int criticalChance, TeamType team,int baseHealth)
    {
        this.baseHealth = health;
        this.name = name;
        this.id = id;
        this.health = health;
        this.turnSpeed = turnSpeed;
        this.armor = armor;
        this.criticalChance = criticalChance;
        this.team = team;
    }
    public void ReduceAllSkillCooldowns()
    {
        foreach (Skill skill in GetSkills())
        {
            skill.ReduceCooldown();
        }
    }
    public HeroData ToHeroData()
    {
        return new HeroData(this);
    }

    public void LoadFromHeroData(HeroData data)
    {
        if (data != null)
            data.ApplyToHero(this);
    }
}

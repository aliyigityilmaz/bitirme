using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public Image healthBar;
    public TextMeshProUGUI healthChangeText;
    public int turnSpeed;
    public int armor;
    public int criticalChance;
    public Skill[] skills;
    public Animator charAnimator;
    public ParticleSystem heroHitVFX;
    public Transform heroTransform;
    public Skill[] GetSkills() { return skills; }

    public Hero(string name, int id, int health, int turnSpeed, int armor, int criticalChance, TeamType team, int baseHealth, Image healthBar, TextMeshProUGUI healthChangeText)
    {
        this.baseHealth = baseHealth;
        this.name = name;
        this.id = id;
        this.health = health;
        this.turnSpeed = turnSpeed;
        this.armor = armor;
        this.criticalChance = criticalChance;
        this.team = team;
        this.healthBar = healthBar;
        this.healthChangeText = healthChangeText;
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
    public void UpdateHealthBar()
    {
        if (healthBar != null && baseHealth > 0)
        {
            healthBar.fillAmount = (float)health / baseHealth;
        }
    }
    public void ShowHealthChangeText(int value, bool isHealing)
    {
        if (healthChangeText == null) return;

        healthChangeText.text = (isHealing ? "+" : "-") + Mathf.Abs(value);
        healthChangeText.color = isHealing ? Color.green : Color.red;

        healthChangeText.gameObject.SetActive(true);

        CombatStateManager.Instance.StartCoroutine(HideHealthChangeText());
    }

    private IEnumerator HideHealthChangeText()
    {
        yield return new WaitForSeconds(4f);
        healthChangeText.gameObject.SetActive(false);
    }
}

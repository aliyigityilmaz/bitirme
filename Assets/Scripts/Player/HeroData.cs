[System.Serializable]
public class HeroData
{
    public int id;
    public string name;
    public int baseHealth;
    public int health;
    public int armor;
    public int turnSpeed;
    public int criticalChance;

    public HeroData(int id, string name, int baseHealth, int health, int armor, int turnSpeed, int criticalChance)
    {
        this.id = id;
        this.name = name;
        this.baseHealth = baseHealth;
        this.health = health;
        this.armor = armor;
        this.turnSpeed = turnSpeed;
        this.criticalChance = criticalChance;
    }

    public HeroData(Hero hero)
    {
        this.id = hero.id;
        this.name = hero.name;
        this.baseHealth = hero.baseHealth;
        this.health = hero.health;
        this.armor = hero.armor;
        this.turnSpeed = hero.turnSpeed;
        this.criticalChance = hero.criticalChance;
    }

    public void ApplyToHero(Hero hero)
    {
        hero.id = this.id;
        hero.name = this.name;
        hero.baseHealth = this.baseHealth;
        hero.health = this.health;
        hero.armor = this.armor;
        hero.turnSpeed = this.turnSpeed;
        hero.criticalChance = this.criticalChance;
    }
}
[System.Serializable]
public class HeroSaveData
{
    public int id;
    public int health;
    public int armor;
    public int turnSpeed;
    public int criticalChance;
    public int baseHealth;
    

    public HeroSaveData(Hero hero)
    {
        id = hero.id;
        health = hero.health;
        baseHealth = hero.baseHealth;
        armor = hero.armor;
        turnSpeed = hero.turnSpeed;
        criticalChance = hero.criticalChance;
        
    }
}
[System.Serializable]
public class Hero 
{
    public string name;
    public int id;
    public int health;
    public int turnSpeed;
    public int armor;
    public int criticalChance;

    public Hero(string name, int id, int health, int turnSpeed, int armor, int criticalChance)
    {
        this.name = name;
        this.id = id;
        this.health = health;
        this.turnSpeed = turnSpeed;
        this.armor = armor;
        this.criticalChance = criticalChance;
    }

}

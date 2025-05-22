using System.Collections.Generic;

public static class HeroSaveManager
{
    public static Dictionary<int, HeroSaveData> heroSaves = new Dictionary<int, HeroSaveData>();

    public static void SaveHeroes(List<Hero> heroList)
    {
        heroSaves.Clear();
        foreach (var hero in heroList)
        {
            heroSaves[hero.id] = new HeroSaveData(hero);
        }
    }

    public static void LoadHeroes(List<Hero> heroList)
    {
        foreach (var hero in heroList)
        {
            if (heroSaves.ContainsKey(hero.id))
            {
                var data = heroSaves[hero.id];
                hero.health = data.health;
                hero.baseHealth = data.baseHealth;
                hero.armor = data.armor;
                hero.turnSpeed = data.turnSpeed;
                hero.criticalChance = data.criticalChance;
                
            }
        }
    }
}
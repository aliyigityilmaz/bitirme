using UnityEngine;

public class OpenWorldHeroDebugLog : MonoBehaviour
{
    void Update()
    {
        var hero = HeroPersistent.instance.GetHeroDataById(1);
        if (hero == null)
        {
            Debug.LogWarning("ID 1 olan Hero verisi bulunamadı.");
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hero.health += 10;
            Debug.Log($"[TEST] Hero {hero.name} canı arttı: {hero.health}");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            hero.armor += 5;
            Debug.Log($"[TEST] Hero {hero.name} zırhı arttı: {hero.armor}");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            hero.health -= 15;
            Debug.Log($"[TEST] Hero {hero.name} canı AZALDI: {hero.health}");
        }
    }
}
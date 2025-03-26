using System.Linq;
using UnityEngine;

public class PlayerInputState : ICombatState
{
    private CombatStateManager manager;

    // Kullanılacak harfler dizisi
    private char[] allowedKeys = new char[] { 'A', 'S', 'D', 'F' };
    private char targetKey;         // Seçilen hedef harf
    private float letterDisplayTime; // Harf gösterildiği an
    private bool inputProcessed = false;

    // İdeal reaksiyon süresi (saniye cinsinden)
    private float idealReactionTime = 1.0f;
    // Otomatik işlem süresi (örneğin 2 saniye, oyuncu basmazsa)
    private float maxReactionTime = 2.0f;

    public PlayerInputState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("Entering Player Input State (harf tabanlı timing)");
        // Rastgele bir harf seçiyoruz
        int index = Random.Range(0, allowedKeys.Length);
        targetKey = allowedKeys[index];
        letterDisplayTime = Time.time;
        inputProcessed = false;

        // Hangi harfe basılacağı console’da yazdırılıyor
        Debug.Log("Basmanız gereken harf: " + targetKey);

        // UI'ye bağlı Text ve Slider'ı ayarla
        SkillUIManager.Instance.SliderConnect(targetKey, maxReactionTime);
    }

    public void Execute()
    {
        if (!inputProcessed)
        {
            // Geçen süreyi hesapla ve slider'ı güncelle
            float elapsedTime = Time.time - letterDisplayTime;
            SkillUIManager.Instance.UpdateSlider(elapsedTime);

            // Seçilen harfi KeyCode olarak elde ediyoruz
            KeyCode targetKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), targetKey.ToString());

            // Eğer oyuncu doğru harfe basarsa
            if (Input.GetKeyDown(targetKeyCode))
            {
                float reactionTime = elapsedTime;
                ProcessInput(reactionTime);
            }
            // Maksimum süreden sonra otomatik düşük vurma uygulasın
            else if (elapsedTime > maxReactionTime)
            {
                Debug.Log("Zaman aşımı! Otomatik olarak düşük vurma uygulanıyor.");
                ProcessInput(maxReactionTime);
            }
        }
    }

    private void ProcessInput(float reactionTime)
    {
        inputProcessed = true;
        float multiplier;

        // Zaman aralıklarına göre multiplikatör belirliyoruz
        if (reactionTime >= 0.9f && reactionTime <= 1.1f)
        {
            multiplier = 1.5f;
            Debug.Log("Perfect timing! (Mükemmel vurma)");
        }
        else if ((reactionTime >= 0.7f && reactionTime < 0.9f) || (reactionTime > 1.1f && reactionTime <= 1.3f))
        {
            multiplier = 1.0f;
            Debug.Log("Normal timing! (Normal vurma)");
        }
        else
        {
            multiplier = 0.5f;
            Debug.Log("Poor timing! (Zayıf vurma)");
        }

        // Aktif kahraman ve seçilen yetenek bilgilerini alıyoruz.
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        Debug.Log($"Selected skill index in Input: {manager.selectedSkillIndex}");

        if (activeHero == null)
        {
            Debug.LogError("Error: activeHero is null!");
        }

        Skill selectedSkill = activeHero.GetSkills()[manager.selectedSkillIndex];
        manager.selectedSkill = selectedSkill;

        // Final değeri (hasar veya iyileştirme) hesaplıyoruz.
        float finalValue = selectedSkill.baseDamage * multiplier;

        if (selectedSkill.skillType == SkillType.Damage)
        {
            if (manager.selectedEnemy != null)
            {
                manager.selectedEnemy.health -= (int)finalValue;
                Debug.Log($"{activeHero.name} {manager.selectedEnemy.name} üzerinde {(int)finalValue} hasar yaptı.");
            }
        }
        else if (selectedSkill.skillType == SkillType.Heal)
        {
            if (manager.selectedEnemy != null)
            {
                manager.selectedEnemy.health += (int)finalValue;
                Debug.Log($"{activeHero.name} {manager.selectedEnemy.name} üzerinde {(int)finalValue} iyileştirme yaptı.");
            }
        }

        // İşlem tamamlandıktan sonra sonraki duruma geçiyoruz.
        manager.SetState(new PlayerActionState(manager));
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Input State");
        // UI elemanlarını kapatıyoruz
        SkillUIManager.Instance.HideSlider();
    }
}
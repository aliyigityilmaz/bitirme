using System.Linq;
using UnityEngine;

public class PlayerInputState : ICombatState
{
    private CombatStateManager manager;
    public static PlayerInputState instance;
    // Kullanılacak harfler dizisi
    private char[] allowedKeys = new char[] { 'A', 'S', 'D', 'F' };
    private char targetKey;         // Seçilen hedef harf
    private float letterDisplayTime; // Harf gösterildiği an
    private bool inputProcessed = false;

    // İdeal reaksiyon süresi (saniye cinsinden)
    private float idealReactionTime = 1.0f;
    // Otomatik işlem süresi (örneğin 2 saniye, oyuncu basmazsa)
    private float maxReactionTime = 2.0f;

    // Birden fazla tuş basışı için:
    // Gereken tuş sayısı = seçilen skillin index + 1 (örneğin 1. skill için 1, 3. skill için 3)
    private int requiredPresses;
    private int currentPressCount;
    private float totalMultiplier;
    public float finalDamage;
    public PlayerInputState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        instance = this;
        Debug.Log("Entering Player Input State (harf tabanlı timing - multi press)");

        // Seçilen skillin index'ine göre kaç basış gerektiğini belirle (index 0 ise 1 basış, index 2 ise 3 basış)
        requiredPresses = manager.selectedSkillIndex + 1;
        currentPressCount = 0;
        totalMultiplier = 0f;

        // İlk round'u başlat
        SetupNextRound();
    }

    private void SetupNextRound()
    {
        // Her round için rastgele yeni bir harf seç ve zamanı sıfırla
        int index = Random.Range(0, allowedKeys.Length);
        targetKey = allowedKeys[index];
        letterDisplayTime = Time.time;
        inputProcessed = false;

        // UI'yi güncelle (Slider ve Text)
        SkillUIManager.Instance.SliderConnect(targetKey, maxReactionTime);
        Debug.Log("Round " + (currentPressCount + 1) + " of " + requiredPresses + ": Basmanız gereken harf: " + targetKey);
    }

    public void Execute()
    {
        if (!inputProcessed)
        {
            float elapsedTime = Time.time - letterDisplayTime;
            SkillUIManager.Instance.UpdateSlider(elapsedTime);

            KeyCode targetKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), targetKey.ToString());
            if (Input.GetKeyDown(targetKeyCode))
            {
                ProcessRound(elapsedTime);
            }
            else if (elapsedTime > maxReactionTime)
            {
                Debug.Log("Zaman aşımı! Otomatik olarak düşük vurma uygulanıyor.");
                ProcessRound(maxReactionTime);
            }
        }
    }

    private void ProcessRound(float reactionTime)
    {
        inputProcessed = true;
        float multiplier;

        // Zaman aralıklarına göre bu round için multiplikatör hesapla
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

        totalMultiplier += multiplier;
        currentPressCount++;

        if (currentPressCount < requiredPresses)
        {
            // Eğer daha fazla basış gerekiyorsa yeni round başlat
            SetupNextRound();
        }
        else
        {
            // Tüm round'lar tamamlandığında, ortalama multiplier'ı hesapla
            float finalMultiplier = totalMultiplier / requiredPresses;
            Debug.Log("Tüm roundlar tamamlandı. Final multiplier: " + finalMultiplier);
            finalDamage = finalMultiplier;
            manager.SetState(new PlayerActionState(manager));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Input State");
        SkillUIManager.Instance.HideSlider();
    }
}

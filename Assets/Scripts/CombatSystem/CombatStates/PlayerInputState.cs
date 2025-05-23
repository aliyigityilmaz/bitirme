using System.Linq;
using UnityEngine;

public class PlayerInputState : ICombatState
{
    public static PlayerInputState instance;
    private CombatStateManager manager;
    // Kullanılacak tuşlar: W, A, S, D
    private char[] allowedKeys = new char[] { 'W', 'A', 'S', 'D' };
    private char targetKey;         // Seçilen tuş
    private float roundStartTime;   // Round başlangıç zamanı
    private bool inputProcessed = false;

    // Şarkı notaları – her kahramanın kendine ait 6 notası; burada ilk 3 nota kullanılacak.
    private float[] songNotesTime;
    private float currentNoteDuration; // Round için seçilen nota süresi
    private AudioClip[] songNotes;
    private AudioClip[] songs;

    // Round sayısı (örnekte 2-6 arası rastgele belirleniyor fakat ilk 3 nota kullanılacağı için sınırlandırıyoruz)
    private int requiredPresses;
    private int currentPressCount;
    private float totalMultiplier;
    private CombatCameraManager combatCameraManager;
    private AudioForCombat afc;
    private AudioClip currentAudioClip;
    public float finalDamage;
    

    public PlayerInputState(CombatStateManager manager)
    {
        this.manager = manager;
    }
    
    public void Enter()
    {
        instance = this;
        afc = AudioForCombat.Instance;
        Debug.Log("Yeni ritim mekaniği ile PlayerInputState'e girildi");
        afc.PlayMusicWithCrossFade(afc.combatMusicClip2,0f);
        combatCameraManager = CombatCameraManager.instance;

        // Turn-based sistemde sıradaki aktif kahraman alınır.
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];

        // Aktif kahramanın kendine özgü şarkı notalarını almak için HeroSongNotes kullanılır.
        songNotesTime = HeroSongNotes.GetSongNotesForHero(activeHero);
        if (activeHero.id == 1)
        {
            songNotes = afc.heroMainNotes;
            songs = afc.heroMainSong;
            combatCameraManager.SetCameraPosition(combatCameraManager.cameraTransformHero1);
        }

        if (activeHero.id == 2)
        {
            songNotes = afc.heroSniperNotes;
            songs = afc.heroSniperSong;
            combatCameraManager.SetCameraPosition(combatCameraManager.cameraTransformHero2);
        }

        if (activeHero.id == 3)
        {
            songNotes = afc.heroTankNotes;
            songs = afc.heroTankSong;
            combatCameraManager.SetCameraPosition(combatCameraManager.cameraTransformHero3);
        }

        if (activeHero.id == 4)
        {
            songNotes = afc.heroHealerNotes;
            songs = afc.heroHealerSong;
            combatCameraManager.SetCameraPosition(combatCameraManager.cameraTransformHero4);
        }
        // Eğer yeterli nota yoksa default bir dizi tanımlanır.
        if (songNotesTime == null || songNotesTime.Length < 3)       
        {
            songNotesTime = new float[] { 1.0f, 1.2f, 0.8f, 1.0f, 1.1f, 0.9f }; 
            
        }
        

        // Örnekte, round sayısını 2 ile 6 arasında rastgele belirleyip 3 round ile sınırlandırıyoruz.
        requiredPresses = 6;
        currentPressCount = 0; 
        totalMultiplier = 0f;

        SetupNextRound();
        

    }

    
    
    private void SetupNextRound()
    {
        // Round başında, rastgele bir tuş seçilir.
        int keyIndex = Random.Range(0, allowedKeys.Length);
        targetKey = allowedKeys[keyIndex];
        roundStartTime = Time.time;
        inputProcessed = false;

        // Aktif round için nota süresi, kahramanın şarkı notası dizisindeki ilgili notadan alınır.
        currentNoteDuration = songNotesTime[currentPressCount];
        currentAudioClip=songNotes[currentPressCount];

        // SkillUIManager üzerinden UI ayarları yapılır:
        // SliderConnect metodu, tuş metnini (ör. "Press W") günceller ve slider'ın minimum, maksimum değerlerini ayarlar.
        SkillUIManager.Instance.SliderConnect(targetKey, currentNoteDuration);
        Debug.Log("Round " + (currentPressCount + 1) + " of " + requiredPresses + 
                  ": Basılması gereken tuş: " + targetKey + ", Nota süresi: " + currentNoteDuration);
    }

    public void Execute()
    {
        if (!inputProcessed)
        {
            float elapsedTime = Time.time - roundStartTime;
            // Round boyunca slider’ın değeri güncellenir.
            SkillUIManager.Instance.UpdateSlider(elapsedTime);

            // Seçilen tuş kontrol edilir.
            KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), targetKey.ToString());
            if (Input.GetKeyDown(keyCode))
            {
                ProcessRound(currentNoteDuration, elapsedTime);
                afc.PlayNote(currentAudioClip);
            }
            else if (elapsedTime > currentNoteDuration)
            {
                Debug.Log("Zaman aşımı! Otomatik olarak zayıf vurma uygulanıyor.");
                ProcessRound(currentNoteDuration, currentNoteDuration);
            }
        }
    }

    private void ProcessRound(float noteDuration, float elapsedTime)
    {
        inputProcessed = true;
        // İdeal vuruş zamanı, nota süresinin tam yarısıdır.
        float deviation = Mathf.Abs(elapsedTime - (noteDuration / 2f));
        float multiplier;

        if (deviation <= noteDuration * 0.1f)
        {
            multiplier = 1.5f;
            Debug.Log("Perfect timing! (Mükemmel vurma)");
        }
        else if (deviation <= noteDuration * 0.2f)
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
            SetupNextRound();
        }
        else
        {
            float finalMultiplier = totalMultiplier / requiredPresses;
            Debug.Log("Tüm roundlar tamamlandı. Final multiplier: " + finalMultiplier);
            if (1f<finalMultiplier && finalMultiplier<=1.5)
            {
                afc.PlayMusicWithCrossFade(songs[2]);
            }
            if (0.5<finalMultiplier && finalMultiplier<=1)
            {
                afc.PlayMusicWithCrossFade(songs[1]);
            }
            if ( finalMultiplier <=0.5)
            {
                afc.PlayMusicWithCrossFade(songs[0]);
            }
            finalDamage = finalMultiplier;
            manager.SetState(new PlayerActionState(manager));
            
        }
    }
    

    

    public void Exit()
    {
        Debug.Log("PlayerInputState'ten çıkılıyor");
        SkillUIManager.Instance.HideSlider();
    }
}
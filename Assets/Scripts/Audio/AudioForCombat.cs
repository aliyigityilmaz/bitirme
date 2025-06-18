using System;
using UnityEngine;
using System.Collections;


public class AudioForCombat : MonoBehaviour
{
    #region Static Instance
    private static AudioForCombat instance;

    public static AudioForCombat Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioForCombat>();
            {
                if (instance == null)
                {
                    instance = new GameObject("SpawnedAudioManager", typeof(AudioForCombat))
                        .GetComponent<AudioForCombat>();
                }
            }
            return instance;
        }
        private set { instance = value; }
    }
    #endregion

    [Header("Sources")]
    public AudioSource combatMusicSource1;
    public AudioSource combatMusicSource2;
    public AudioSource combatNoteSource;
    public AudioSource combatSfxSource;
    [Header("Main Combat Sounds")]
    public AudioClip combatMusicClip1;
    public AudioClip combatMusicClip2;
    [Header("HeroMainSounds")]
    public AudioClip[] heroMainSong;
    public AudioClip[] heroMainNotes;

    [Header("HeroSniperSounds")]
    public AudioClip[] heroSniperSong;
    public AudioClip[] heroSniperNotes;

    [Header("HeroTankSounds")]
    public AudioClip[] heroTankSong;
    public AudioClip[] heroTankNotes;

    [Header("HeroHealerSounds")]
    public AudioClip[] heroHealerSong;
    public AudioClip[] heroHealerNotes;

    private bool firstMusicSourcePlayin;


    private void Start()
    {
        combatMusicSource1.loop = true;
        combatMusicSource2.loop = true;
    }

    #region One‑shot SFX
    // sadece nota için
    public void PlayNote(AudioClip clip)
    {
        combatNoteSource.PlayOneShot(clip);
    }

    // sadece combat sfx için
    public void PlaySfx(AudioClip clip)
    {
        combatSfxSource.PlayOneShot(clip);
    }
    #endregion

    #region Instant play (no fade)
    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (firstMusicSourcePlayin) ? combatMusicSource1 : combatMusicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = 1f;
        activeSource.Play();
    }
    #endregion

    

    
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1f)
    {
        AudioSource activeSource = firstMusicSourcePlayin ? combatMusicSource1 : combatMusicSource2;
        StartCoroutine(FadeOutIn(activeSource, newClip, transitionTime));
    }

    
    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime = 0.3f)
    {
        AudioSource from = firstMusicSourcePlayin ? combatMusicSource1 : combatMusicSource2;
        AudioSource to   = firstMusicSourcePlayin ? combatMusicSource2 : combatMusicSource1;

        to.clip = newClip;
        StartCoroutine(CrossFade(from, to, transitionTime));

        // roll the flag so bir sonraki çağrıda kaynaklar yer değiştirsin
        firstMusicSourcePlayin = !firstMusicSourcePlayin;
    }

    private IEnumerator FadeOutIn(AudioSource src, AudioClip newClip, float time)
    {
        float startVol = src.volume;
        float t = 0f;

        // Fade‑out
        while (t < time)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(startVol, 0f, t / time);
            yield return null;
        }

        src.Stop();
        src.clip = newClip;
        src.Play();

        // Fade‑in
        t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, startVol, t / time);
            yield return null;
        }
    }

    private IEnumerator CrossFade(AudioSource from, AudioSource to, float time)
    {
        to.volume = 0f;
        to.Play();

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float k = t / time; // 0 ➜ 1
            from.volume = Mathf.Lerp(1f, 0f, k);
            to.volume   = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }

        from.Stop();
    }

    
}

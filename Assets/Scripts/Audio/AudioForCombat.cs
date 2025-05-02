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
    [Header("HeroMainSounds")]
    public AudioClip heroMainSong;
    public AudioClip []heroMainNotes;
    [Header("HeroSniperSounds")]
    public AudioClip heroSniperSong;
    public AudioClip []heroSniperNotes;
    [Header("HeroTankSounds")]
    public AudioClip heroTankSong;
    public AudioClip []heroTankNotes;
    [Header("HeroHealerSounds")]
    public AudioClip heroHealerSong;
    public AudioClip []heroHealerNotes;
    private bool firstMusicSourcePlayin;


    private void Start()
    {
        combatMusicSource1.loop = true;
        combatMusicSource2.loop = true;
    }

    public void PlayNote(AudioClip clip)
    {
        combatNoteSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (firstMusicSourcePlayin) ? combatMusicSource1 : combatMusicSource2;
            
        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip newClip, float tranitionTime = 1.0f)
    {
        AudioSource activeSource = (firstMusicSourcePlayin) ?combatMusicSource1 : combatMusicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, tranitionTime));
    }
     
    IEnumerator UpdateMusicWithFade(AudioSource activeSource,AudioClip newClip,float transationTime)
    {
        //if (!activeSource.isPlaying)
        //activeSource.Play();
            
        float t = 0.0f;

        for (t = 0;  t< transationTime; t+=Time.deltaTime)
        {
            activeSource.volume =  (t / transationTime);
            yield return null;
        }
            
        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();
    }
}

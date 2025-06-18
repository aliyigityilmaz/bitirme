using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Tüm sahnelerde çalýþsýn
        }
        else
        {
            Destroy(gameObject);
        }

        if( musicSource != null || sfxSource != null)
        {
            musicSource.volume = PlayerPrefs.GetInt("MusicVolume", 5) / 10f;
            sfxSource.volume = PlayerPrefs.GetInt("SFXVolume", 5) / 10f;
        }
    }

    #region Volume Controls
    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }
    #endregion

    #region Music
    public void PlayMusic(int index, bool loop = true)
    {
        if (musicClips == null || index < 0 || index >= musicClips.Length) return;

        musicSource.clip = musicClips[index];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
    #endregion

    #region SFX
    public void PlaySFX(int index)
    {
        if (sfxClips == null || index < 0 || index >= sfxClips.Length) return;

        sfxSource.PlayOneShot(sfxClips[index]);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
    #endregion

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempSFX");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f; // 3D ses
        source.minDistance = 3f;
        source.maxDistance = 20f;
        source.Play();

        Destroy(tempGO, clip.length);
    }

}

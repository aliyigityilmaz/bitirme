using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio Settings")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Resolution Settings")]
    public TextMeshProUGUI resolutionText;
    private string[] resolutionOptions = {
        "2560x1440", "1920x1080", "1366x768", "1280x720", "1024x768", "800x600", "640x480"
    };
    private int currentResolutionIndex = 0;

    [Header("Graphics Settings")]
    public TextMeshProUGUI qualityText;
    private readonly string[] qualityLevels = { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };
    private int fakeQualityIndex = 0;

    private bool isFullscreen = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings();
    }

    void Start()
    {
        // Slider ayarlarý
        musicSlider.minValue = 0;
        musicSlider.maxValue = 10;
        musicSlider.wholeNumbers = true;

        sfxSlider.minValue = 0;
        sfxSlider.maxValue = 10;
        sfxSlider.wholeNumbers = true;

        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateSFXVolume(); });

        UpdateResolutionText();
        UpdateQualityText();
    }

    // Ses
    public void UpdateMusicVolume()
    {
        float volume = musicSlider.value / 10f;
        AudioManager.Instance?.SetMusicVolume(volume);
        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
    }

    public void UpdateSFXVolume()
    {
        float volume = sfxSlider.value / 10f;
        AudioManager.Instance?.SetSFXVolume(volume);
        PlayerPrefs.SetInt("SFXVolume", (int)sfxSlider.value);
    }

    // Çözünürlük
    public void NextResolution()
    {
        currentResolutionIndex = (currentResolutionIndex + 1) % resolutionOptions.Length;
        ApplyResolution();
    }

    public void PreviousResolution()
    {
        currentResolutionIndex = (currentResolutionIndex - 1 + resolutionOptions.Length) % resolutionOptions.Length;
        ApplyResolution();
    }

    private void ApplyResolution()
    {
        string[] parts = resolutionOptions[currentResolutionIndex].Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);
        Screen.SetResolution(width, height, isFullscreen);

        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);

        UpdateResolutionText();
    }

    private void UpdateResolutionText()
    {
        resolutionText.text = resolutionOptions[currentResolutionIndex];
    }

    // Kalite
    public void NextQuality()
    {
        fakeQualityIndex = (fakeQualityIndex + 1) % qualityLevels.Length;
        UpdateQualityText();
        PlayerPrefs.SetInt("QualityLevelIndex", fakeQualityIndex);
    }

    public void PreviousQuality()
    {
        fakeQualityIndex = (fakeQualityIndex - 1 + qualityLevels.Length) % qualityLevels.Length;
        UpdateQualityText();
        PlayerPrefs.SetInt("QualityLevelIndex", fakeQualityIndex);
    }

    private void UpdateQualityText()
    {
        if (qualityLevels != null && fakeQualityIndex >= 0 && fakeQualityIndex < qualityLevels.Length)
        {
            qualityText.text = qualityLevels[fakeQualityIndex];
        }
        else
        {
            qualityText.text = "Unknown";
        }
    }

    // Fullscreen
    public void SetFullscreen()
    {
        isFullscreen = !Screen.fullScreen;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
        Debug.Log("Fullscreen toggled: " + isFullscreen);
    }

    // Ayarlarý yükle
    private void LoadSettings()
    {
        // Ses
        int musicVal = PlayerPrefs.GetInt("MusicVolume", 5);
        int sfxVal = PlayerPrefs.GetInt("SFXVolume", 5);
        musicSlider.value = musicVal;
        sfxSlider.value = sfxVal;
        AudioManager.Instance?.SetMusicVolume(musicVal / 10f);
        AudioManager.Instance?.SetSFXVolume(sfxVal / 10f);

        // Çözünürlük
        int width = PlayerPrefs.GetInt("ResolutionWidth", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("ResolutionHeight", Screen.currentResolution.height);
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        Screen.SetResolution(width, height, Screen.fullScreen);

        // Fullscreen
        isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;

        // Kalite
        fakeQualityIndex = PlayerPrefs.GetInt("QualityLevelIndex", 3);
        UpdateQualityText();
    }
}

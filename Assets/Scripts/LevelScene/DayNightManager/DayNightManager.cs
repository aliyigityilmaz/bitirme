using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager Instance;

    [Header("Time Settings")]
    [Range(0f, 24f)] public float currentTime = 12f;
    public float timeSpeed = 1f; // 60 = 1 dakikada 1 saat geçer

    [Header("Lighting")]
    private Light directionalLight;
    public Gradient lightColorOverDay;
    public AnimationCurve lightIntensityOverDay;

    [Header("UI")]
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI targetTimeText;
    public Button confirmButton;

    private float? targetTime = null;

    private bool isTransitioningTime = false;
    private float transitionSpeedMultiplier = 10f;
    private float normalTimeSpeed;

    [Header("Clock Hands")]
    public Transform hourHand;
    public Transform minuteHand;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        directionalLight = FindObjectOfType<Light>();

        confirmButton.onClick.AddListener(ConfirmTimeChange);
        targetTimeText.text = "00:00";
        normalTimeSpeed = timeSpeed;
    }

    void Update()
    {
        UpdateClockHands();

        if (isTransitioningTime && targetTime.HasValue)
        {
            float previousTime = currentTime;
            currentTime = (currentTime + Time.deltaTime * timeSpeed + 24f) % 24f;

            // Hedefi geçip geçmediðini kontrol et
            float deltaBefore = (targetTime.Value - previousTime + 24f) % 24f;
            float deltaAfter = (targetTime.Value - currentTime + 24f) % 24f;

            bool passedTarget = deltaAfter > deltaBefore; // zaman hedefin "önünden geçtiyse"

            if (passedTarget || deltaAfter < 0.1f)
            {
                currentTime = targetTime.Value;
                targetTime = null;
                isTransitioningTime = false;
                timeSpeed = normalTimeSpeed;
                targetTimeText.text = "00:00";
            }

            UpdateLighting();
            UpdateCurrentTimeText();
            return;
        }


        // Normal zaman akýþý
        currentTime = (currentTime + Time.deltaTime * timeSpeed / 60f) % 24f;

        UpdateLighting();
        UpdateCurrentTimeText();
    }


    void UpdateClockHands()
    {

        //const float hourOffset = 270f; // 9 * 30°
        //const float minuteOffset = 0f; // Eðer yelkovan düzgünse bunu deðiþtirme
        // Saat UI'sine göre 0:00 = 180°, yani 180 derece offset
        const float rotationOffset = 90f; // 0:00 aþaðýyý göstersin
        if (hourHand != null)
        {
            float hourRotation = -(currentTime * 15f) + rotationOffset;
            hourHand.localRotation = Quaternion.Euler(0f, 0f, hourRotation);
        }


        if (minuteHand != null)
        {
            float minutes = (currentTime % 1f) * 60f;
            float minuteRotation = -(minutes * 6f);
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, minuteRotation);
        }




    }

    void UpdateCurrentTimeText()
    {
        int hour = Mathf.FloorToInt(currentTime);
        int minute = Mathf.FloorToInt((currentTime - hour) * 60);
        currentTimeText.text = $"{hour:00}:{minute:00}";
    }
    int GetShortestDirection(float from, float to)
    {
        float delta = (to - from + 24f) % 24f;
        return delta <= 12f ? 1 : -1;
    }


    void UpdateLighting()
    {
        float normalizedTime = currentTime / 24f;

        if (directionalLight != null)
        {
            directionalLight.color = lightColorOverDay.Evaluate(normalizedTime);
            directionalLight.intensity = lightIntensityOverDay.Evaluate(normalizedTime);
            directionalLight.transform.rotation = Quaternion.Euler((normalizedTime * 360f) - 90f, 170f, 0f);
        }
    }

    public void SetTargetTime(float hour)
    {
        if (IsTimeTransitioning()) return;
        targetTime = hour;
        targetTimeText.text = hour + ":00";
    }

    public void ConfirmTimeChange()
    {
        if (IsTimeTransitioning()) return;
        if (targetTime.HasValue)
        {
            isTransitioningTime = true;
            timeSpeed = normalTimeSpeed * transitionSpeedMultiplier;
        }
    }

    public bool IsTimeTransitioning()
    {
        return isTransitioningTime;
    }

    // Yardýmcý fonksiyonlar
    public bool IsMidnightToMorning() => currentTime >= 0f && currentTime < 6f;
    public bool IsMorningToNoon() => currentTime >= 6f && currentTime < 12f;
    public bool IsNoonToEvening() => currentTime >= 12f && currentTime < 18f;
    public bool IsEveningToMidnight() => currentTime >= 18f && currentTime < 24f;
}
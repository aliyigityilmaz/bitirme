using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatTutorialManager : MonoBehaviour
{
    public static CombatTutorialManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject step1Panel;
    [SerializeField] private GameObject step2Panel;
    [SerializeField] private Toggle    step2Toggle;
    

    private bool Step1Shown;
    private bool Step2Shown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        tutorialCanvas.SetActive(false);
        step1Panel.SetActive(false);
        step2Panel.SetActive(false);
    }

    /// <summary>
    /// EncounterManager’dan çağrılacak wrapper.
    /// </summary>
    public void StartTutorial()
    {
        // bayrakları sıfırla (eğer yeniden göstermek gerekirse)
        Step1Shown = false;
        Step2Shown = false;
        ShowStep1();
    }

    public void ShowStep1()
    {
        if (Step1Shown) return;
        Step1Shown = true;
        tutorialCanvas.SetActive(true);
        step1Panel.SetActive(true);
        StartCoroutine(WaitForMouseClickToHideStep1());
    }

    private IEnumerator WaitForMouseClickToHideStep1()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        step1Panel.SetActive(false);
        tutorialCanvas.SetActive(false);
    }

    public void ShowStep2()
    {
        if (Step2Shown) return;
        Step2Shown = true;
        tutorialCanvas.SetActive(true);
        step2Panel.SetActive(true);
        Time.timeScale = 0f;
        step2Toggle.isOn = false;
        step2Toggle.onValueChanged.AddListener(OnStep2Toggled);
    }

    private void OnStep2Toggled(bool isOn)
    {
        if (!isOn) return;
        step2Toggle.onValueChanged.RemoveListener(OnStep2Toggled);
        step2Panel.SetActive(false);
        tutorialCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void HideTutorial()
    {
        tutorialCanvas.SetActive(false);
        step1Panel.SetActive(false);
        step2Panel.SetActive(false);
        Time.timeScale = 1f;
        step2Toggle.onValueChanged.RemoveListener(OnStep2Toggled);
    }
}

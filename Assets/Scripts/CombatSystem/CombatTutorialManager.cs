using System.Collections;
using UnityEngine;

public class CombatTutorialManager : MonoBehaviour
{
    public static CombatTutorialManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject step1Panel;
    [SerializeField] private GameObject step2Panel;

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
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void StartTutorial()
    {
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
        StartCoroutine(WaitForAnyKeyToHideStep(step1Panel));
    }

    public void ShowStep2()
    {
        if (Step2Shown) return;
        Step2Shown = true;
        tutorialCanvas.SetActive(true);
        step2Panel.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(WaitForAnyKeyToHideStep(step2Panel, true));
    }

    private IEnumerator WaitForAnyKeyToHideStep(GameObject panel, bool resumeTime = false)
    {
        yield return null; // frame bekle

        while (!Input.anyKeyDown)
            yield return null;

        panel.SetActive(false);
        tutorialCanvas.SetActive(false);

        if (resumeTime)
            Time.timeScale = 1f;
    }

    public void HideTutorial()
    {
        tutorialCanvas.SetActive(false);
        step1Panel.SetActive(false);
        step2Panel.SetActive(false);
        Time.timeScale = 1f;
    }
}
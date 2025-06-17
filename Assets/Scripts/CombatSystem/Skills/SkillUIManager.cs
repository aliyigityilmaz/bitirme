using CombatSystem.CombatStates;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager Instance;
    public GameObject skillPanel;
    public Button[] skillButtons;

    // PlayerInputState i�in eklenen UI elemanlar�

    // public Slider timingSlider;    // Zamanlamay� g�steren slider
    public TimingCircle timingCircle;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitializeSkills(Hero activeHero)
    {
        bool isBossPresent = GameObject.FindGameObjectsWithTag("Boss").Length > 0;

        Skill[] skills = activeHero.GetSkills();
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skills.Length)
            {
                if (i == 3 && !isBossPresent)
                {
                    skillButtons[i].gameObject.SetActive(false);
                    continue;
                }

                int index = i;
                Skill skill = skills[i];

                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"Skill button clicked: {skill.skillName}, index: {index}");
                    OnSkillButtonClicked(index);
                });

                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].interactable = skill.IsAvailable();

                Debug.Log($"Skill Button {i} - {skill.skillName} interactable: {skillButtons[i].interactable}");

                Image buttonImage = skillButtons[i].GetComponent<Image>();
                if (buttonImage != null && skill.skillIcon != null)
                {
                    buttonImage.sprite = skill.skillIcon;
                }
            }
            else
            {
                skillButtons[i].gameObject.SetActive(false);
            }
        }

        skillPanel.SetActive(true);
    }


    public void OnSkillButtonClicked(int index)
    {
        CombatStateManager.Instance.selectedSkillIndex = index;
        Hero activeHero = CombatStateManager.Instance.turnOrder[CombatStateManager.Instance.currentTurnIndex];
        CombatStateManager.Instance.selectedSkill = activeHero.GetSkills()[index];
        CombatStateManager.Instance.IsTargetSelectionActive = true;
        Debug.Log($"Skill Selected: {CombatStateManager.Instance.selectedSkill.skillName}");
    }

    public void TimingCircleConnect(char targetKey, float maxTime)
    {
        if (timingCircle != null)
        {
            timingCircle.ResetCircle();
            timingCircle.StartTiming(targetKey, maxTime);
        }
        else
        {
            Debug.LogWarning("Timing Slider is not assigned in SkillUIManager!");
        }
    }

    public void SetCircleColor(Color color)
    {
        if (timingCircle != null)
        {
            timingCircle.SetCircleColor(color);
        }
        else
        {
            Debug.LogWarning("Timing Circle is not assigned in SkillUIManager!");
        }
    }

    /* PlayerInputState'ten �a�r�larak slider ve text'i ayarlar
    public void SliderConnect(char targetKey, float maxTime)
    {
        if (keyText != null)
        {
            keyText.text = "Press " + targetKey;
        }
        else
        {
            Debug.LogWarning("Key Text is not assigned in SkillUIManager!");
        }

        if (timingSlider != null)
        {
            timingSlider.gameObject.SetActive(true);
            timingSlider.minValue = 0;
            timingSlider.maxValue = maxTime;
            timingSlider.value = 0; // Ba�lang��ta slider de�eri 0 olsun.
        }
        else
        {
            Debug.LogWarning("Timing Slider is not assigned in SkillUIManager!");
        }
    }*/

    /*// Slider'� g�ncellemek i�in metod (PlayerInputState Execute'inde �a�r�l�r)
    public void UpdateSlider(float value)
    {
        if (timingSlider != null)
        {
            timingSlider.value = value;
        }
    }*/

    // ��lem bitti�inde UI elemanlar�n� kapat�r
    public void HideSlider()
    {
        /*if (timingSlider != null)
        {
            timingSlider.gameObject.SetActive(false);
        }*/

        timingCircle.ResetCircle();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager Instance;
    public GameObject skillPanel;
    public Button[] skillButtons;

    // PlayerInputState için eklenen UI elemanlarý
    public Text keyText;           // Hangi tuþa basýlacaðýný gösteren metin
    public Slider timingSlider;    // Zamanlamayý gösteren slider

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitializeSkills(Hero activeHero)
    {
        Skill[] skills = activeHero.GetSkills();
        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < skills.Length)
            { 
                int index = i;
                Skill skill = skills[i];

                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() => OnSkillButtonClicked(index));

                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].interactable = skill.IsAvailable();
                Image buttonImage = skillButtons[i].GetComponent<Image>();
                if (buttonImage != null && skill.skillIcon != null)
                {
                    buttonImage.sprite = skill.skillIcon;
                }
                // Ýsteðe baðlý: Cooldown bilgisi UI’da gösterilebilir.
                //Text buttonText = skillButtons[i].GetComponentInChildren<Text>();
                //if (buttonText != null)
                //{
                //    if (!skill.IsAvailable())
                //        buttonText.text = $"{skill.skillName} ({skill.currentCooldown})";
                //    else
                //        buttonText.text = skill.skillName;
                //}
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

    // PlayerInputState'ten çaðrýlarak slider ve text'i ayarlar
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
            timingSlider.value = 0; // Baþlangýçta slider deðeri 0 olsun.
        }
        else
        {
            Debug.LogWarning("Timing Slider is not assigned in SkillUIManager!");
        }
    }

    // Slider'ý güncellemek için metod (PlayerInputState Execute'inde çaðrýlýr)
    public void UpdateSlider(float value)
    {
        if (timingSlider != null)
        {
            timingSlider.value = value;
        }
    }

    // Ýþlem bittiðinde UI elemanlarýný kapatýr
    public void HideSlider()
    {
        if (timingSlider != null)
        {
            timingSlider.gameObject.SetActive(false);
        }
        if (keyText != null)
        {
            keyText.text = "";
        }
    }
}
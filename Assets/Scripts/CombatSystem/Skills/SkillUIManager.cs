using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager Instance;
    public GameObject skillPanel;
    public Button[] skillButtons;

    // PlayerInputState i�in eklenen UI elemanlar�
    public Text keyText;           // Hangi tu�a bas�laca��n� g�steren metin
    public Slider timingSlider;    // Zamanlamay� g�steren slider

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
                // �ste�e ba�l�: Cooldown bilgisi UI�da g�sterilebilir.
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

    // PlayerInputState'ten �a�r�larak slider ve text'i ayarlar
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
    }

    // Slider'� g�ncellemek i�in metod (PlayerInputState Execute'inde �a�r�l�r)
    public void UpdateSlider(float value)
    {
        if (timingSlider != null)
        {
            timingSlider.value = value;
        }
    }

    // ��lem bitti�inde UI elemanlar�n� kapat�r
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
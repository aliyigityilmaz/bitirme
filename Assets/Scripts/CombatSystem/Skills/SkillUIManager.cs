using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager Instance;
    public GameObject skillPanel;
    public Button[] skillButtons;

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
                int index = i; // Closure sorunu yaþamamak için
                skillButtons[i].onClick.RemoveAllListeners();
                skillButtons[i].onClick.AddListener(() => OnSkillButtonClicked(index));
                skillButtons[i].gameObject.SetActive(true);
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
        CombatStateManager.Instance.IsTargetSelectionActive = true;
    }
}

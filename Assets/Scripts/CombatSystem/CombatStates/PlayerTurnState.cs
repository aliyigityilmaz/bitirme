using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerTurnState : ICombatState
{
    private CombatStateManager manager;
    private AudioForCombat afc;
    

    public PlayerTurnState(CombatStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        afc = AudioForCombat.Instance;
        afc.PlayMusicWithFade(afc.combatMusicClip1);
        CombatTutorialManager.Instance.ShowStep1();
        manager.StartCoroutine(DelayedEnter());
    }

    private IEnumerator DelayedEnter()
    {
        yield return new WaitForSeconds(4f);

        SkillUIManager.Instance.skillPanel.SetActive(true);
        Debug.Log("Entering Player Turn State");
        Hero activeHero = manager.turnOrder[manager.currentTurnIndex];
        SkillUIManager.Instance.InitializeSkills(activeHero);
    }

    public void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manager.SetState(new PlayerInputState(manager));
        }

        foreach (var hero in HeroManager.instance.heroList)
        {
            HeroPersistent.instance.UpdateHeroData(hero);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Player Turn State");
        //SkillUIManager.Instance.skillPanel.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : Interactable
{
    private bool hasInteracted = false;
    public int level = 1;

    [Header("Respawn Settings")] public bool canRespawn = true;
    public float respawnTime = 10f;

    [Header("Spawn Time Window")] public bool useTimeRestrictions = false;
    [Range(0f, 24f)] public float spawnStartTime = 0f;
    [Range(0f, 24f)] public float spawnEndTime = 24f;

    [Header("Drop Settings")] public List<EnemyDrop> dropTable = new List<EnemyDrop>();

    [HideInInspector] public string enemyID;

    private float deathTime;


    private void Awake()
    {
        if (string.IsNullOrEmpty(enemyID))
        {
            enemyID = GenerateUniqueID();
        }
    }

    private string GenerateUniqueID()
    {
        return gameObject.scene.name + "_" + transform.position.ToString();
    }

    private void Start()
    {
        interactableType = InteractableType.Enemy;

        // Kay�t ol
        EnemySpawnManager.Instance.RegisterEnemy(this);
    }

    public override void Interact()
    {
        if (isOneTimeInteraction && hasInteracted)
            return;


        SceneTransitionController.Instance.PlayTransition(() =>
        {
            hasInteracted = true;
            DayNightManager.Instance.timeSpeed = 0f;
            BackpackManager.Instance.SaveBackpack();

            EncounterManager.Instance.SetupEncounterForLevel(level);
            //HeroSaveManager.SaveHeroes(HeroManager.instance.heroList);
            SceneManager.LoadScene("BCombatScene");
        });

        //OnCombatEnded(playerWon: true);
    }

    public void OnCombatEnded(bool playerWon)
    {
        DayNightManager.Instance.timeSpeed = 1f;
        if (playerWon)
        {
            DropItems();
            gameObject.SetActive(false);

            // KAYIT
            EnemySpawnManager.Instance.RegisterDeadEnemy(enemyID);

            if (canRespawn)
            {
                deathTime = Time.time;
                EnemySpawnManager.Instance.RegisterForRespawn(this, deathTime + respawnTime);
            }
        }
    }

    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            float roll = Random.value;
            if (roll <= drop.dropChance)
            {
                int quantity = Random.Range(drop.minQuantity, drop.maxQuantity + 1);
                BackpackManager.Instance.AddItem(drop.itemData, quantity);
            }
        }
    }


    public void Respawn()
    {
        hasInteracted = false;
        gameObject.SetActive(true);
    }

    public bool IsWithinSpawnTime(float currentTime)
    {
        if (!useTimeRestrictions) return true;

        if (spawnStartTime < spawnEndTime)
            return currentTime >= spawnStartTime && currentTime < spawnEndTime;
        else
            return currentTime >= spawnStartTime || currentTime < spawnEndTime;
    }
}

[System.Serializable]
public class EnemyDrop
{
    public InventoryItemData itemData;
    public int minQuantity = 1;
    public int maxQuantity = 1;
    [Range(0f, 1f)] public float dropChance = 1f; // 1 = %100
}


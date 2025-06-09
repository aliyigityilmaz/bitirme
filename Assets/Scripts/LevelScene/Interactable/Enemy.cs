using UnityEngine;

public class Enemy : Interactable
{
    private bool hasInteracted = false;
    public int level = 1;

    [Header("Respawn Settings")]
    public bool canRespawn = true;
    public float respawnTime = 10f;

    [Header("Spawn Time Window")]
    public bool useTimeRestrictions = false;
    [Range(0f, 24f)] public float spawnStartTime = 0f;
    [Range(0f, 24f)] public float spawnEndTime = 24f;

    private float deathTime;

    private void Start()
    {
        interactableType = InteractableType.Enemy;

        // Kayýt ol
        EnemySpawnManager.Instance.RegisterEnemy(this);
    }

    public override void Interact()
    {
        if (isOneTimeInteraction && hasInteracted)
            return;

        hasInteracted = true;

        Debug.Log("Combat baþlatýlýyor... (Combat sistemi henüz yok)");

        OnCombatEnded(playerWon: true);
    }

    public void OnCombatEnded(bool playerWon)
    {
        if (playerWon)
        {
            gameObject.SetActive(false);

            if (canRespawn)
            {
                deathTime = Time.time;
                EnemySpawnManager.Instance.RegisterForRespawn(this, deathTime + respawnTime);
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

using UnityEngine;

public class Enemy : Interactable
{
    private bool hasInteracted = false;
    public int level = 1;

    [Header("Respawn Settings")]
    public bool canRespawn = true;
    public float respawnTime = 10f; // saniye cinsinden

    private float deathTime;

    private void Start()
    {
        interactableType = InteractableType.Enemy;
    }

    public override void Interact()
    {
        if (isOneTimeInteraction && hasInteracted)
            return;

        hasInteracted = true;

        Debug.Log("Combat baþlatýlýyor... (Combat sistemi henüz yok)");

        // Geçici savaþ sonucu
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
                EnemyRespawnManager.Instance.RegisterForRespawn(this, deathTime + respawnTime);
            }
        }
        else
        {
            Debug.Log("Oyuncu kaybetti, düþman aktif kalýyor.");
        }
    }

    public void Respawn()
    {
        hasInteracted = false; // Etkileþim durumu sýfýrlanýr
        gameObject.SetActive(true); // Düþman tekrar sahnede görünür
    }
}

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

        Debug.Log("Combat ba�lat�l�yor... (Combat sistemi hen�z yok)");

        // Ge�ici sava� sonucu
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
            Debug.Log("Oyuncu kaybetti, d��man aktif kal�yor.");
        }
    }

    public void Respawn()
    {
        hasInteracted = false; // Etkile�im durumu s�f�rlan�r
        gameObject.SetActive(true); // D��man tekrar sahnede g�r�n�r
    }
}

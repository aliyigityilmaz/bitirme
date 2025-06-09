using UnityEngine;

public class Enemy : Interactable
{
    private bool hasInteracted = false;
    public int level = 1; // Düþmanýn seviyesi, ileride kullanýlabilir

    private void Start()
    {
        interactableType = InteractableType.Enemy;
    }

    public override void Interact()
    {
        if (isOneTimeInteraction && hasInteracted)
            return;

        hasInteracted = true;

        // TODO: Combat sistemine geçiþ yapýlacak burada.
        Debug.Log("Combat baþlatýlýyor... (Combat sistemi henüz yok)");

        // Þu anlýk otomatik olarak savaþý kazanmýþ gibi düþmaný deaktif yap
        OnCombatEnded(playerWon: true);
    }

    public void OnCombatEnded(bool playerWon)
    {
        if (playerWon)
        {
            // Savaþ kazanýldýysa düþmaný deaktif et
            gameObject.SetActive(false);
        }
        else
        {
            // Savaþ kaybedildiyse bir þey yapma (düþman sahnede kalmaya devam eder)
            Debug.Log("Oyuncu kaybetti, düþman aktif kalýyor.");
        }
    }
}

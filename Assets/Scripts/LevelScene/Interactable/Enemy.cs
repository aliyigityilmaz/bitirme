using UnityEngine;

public class Enemy : Interactable
{
    private bool hasInteracted = false;
    public int level = 1; // D��man�n seviyesi, ileride kullan�labilir

    private void Start()
    {
        interactableType = InteractableType.Enemy;
    }

    public override void Interact()
    {
        if (isOneTimeInteraction && hasInteracted)
            return;

        hasInteracted = true;

        // TODO: Combat sistemine ge�i� yap�lacak burada.
        Debug.Log("Combat ba�lat�l�yor... (Combat sistemi hen�z yok)");

        // �u anl�k otomatik olarak sava�� kazanm�� gibi d��man� deaktif yap
        OnCombatEnded(playerWon: true);
    }

    public void OnCombatEnded(bool playerWon)
    {
        if (playerWon)
        {
            // Sava� kazan�ld�ysa d��man� deaktif et
            gameObject.SetActive(false);
        }
        else
        {
            // Sava� kaybedildiyse bir �ey yapma (d��man sahnede kalmaya devam eder)
            Debug.Log("Oyuncu kaybetti, d��man aktif kal�yor.");
        }
    }
}

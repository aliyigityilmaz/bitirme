using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationNPC : MonoBehaviour
{
    [Header("Animasyon Ayarlar�")]
    private bool isWalking = false;
    public bool isSitting = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // �lk ayarlar� animat�re g�nder
        ApplyAnimationStates();
    }

    void OnValidate()
    {
        // Edit�rde de�i�iklik yap�l�nca hemen uygula
        if (Application.isPlaying && animator != null)
        {
            ApplyAnimationStates();
        }
    }

    void ApplyAnimationStates()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isSitting", isSitting);
    }
}

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationNPC : MonoBehaviour
{
    [Header("Animasyon Ayarlarý")]
    private bool isWalking = false;
    public bool isSitting = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Ýlk ayarlarý animatöre gönder
        ApplyAnimationStates();
    }

    void OnValidate()
    {
        // Editörde deðiþiklik yapýlýnca hemen uygula
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

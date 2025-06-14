using UnityEngine;
using System.Linq;
using System.Collections.Generic; // En yak�n nesneyi bulmak i�in

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float moveSpeed = 5f;
    public float interactionRadius = 2f; // Etkile�im yar��ap�

    private Vector3 moveDirection;
    private HashSet<Interactable> interactedObjects = new HashSet<Interactable>();

    private Animator animator; // Animator bile�eni

    public bool isTalking = false;
    private bool isInteracting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Singleton deseni i�in, ba�ka bir �rnek varsa yok et
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Animator bile�enini al
        if (animator == null)
        {
            Debug.LogError("Animator component not found on PlayerController.");
        }
    }
    void Update()
    {
        if (Time.timeScale == 0 || ESCMenuManager.Instance.menuOpen)
        {
            return;
        }
        if (isTalking)
        {
            animator.SetBool("isTalking", true);
            UpdateAnimator(0f); // Hareket yokken h�z 0 olmal�
            return;
        }
        if (isInteracting) {
            animator.SetBool("isInteracting", true);
            UpdateAnimator(0f); // Hareket yokken h�z 0 olmal�
            return;
        }
        HandleMovement();
        HandleInteraction();
        UpdateInteractableObjects();

        UpdateAnimator(moveDirection.magnitude);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Isometric hareket i�in eksenleri d�n��t�r
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 isoDirection = new Vector3(moveDirection.x - moveDirection.z, 0, moveDirection.x + moveDirection.z);

        if (isoDirection != Vector3.zero)
        {
            transform.forward = isoDirection; // Karakterin hareket y�n�ne bakmas�n� sa�lar
        }

        transform.position += isoDirection * moveSpeed * Time.deltaTime;
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // E�er UI'da aktif bir se�im varsa onunla etkile�ime ge�
            if (InteractableUISelector.Instance != null)
            {
                InteractableUISelector.Instance.TriggerSelected();
                return;
            }

            // Aksi halde fiziksel olarak yak�ndakiyle etkile�ime ge�
            Interactable nearestInteractable = FindNearestInteractable();
            if (nearestInteractable != null)
            {
                nearestInteractable.Interact();

                if (nearestInteractable.isOneTimeInteraction)
                {
                    interactedObjects.Add(nearestInteractable);
                }
            }
        }
    }



    Interactable FindNearestInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        Interactable nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            Interactable interactable = col.GetComponent<Interactable>();
            if (interactable != null && !interactedObjects.Contains(interactable)) // Daha �nce etkile�ime girilmemi�se
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = interactable;
                }
            }
        }
        return nearest;
    }

    void UpdateInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Interactable interactable in FindObjectsOfType<Interactable>())
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance <= interactionRadius)
            {
                InteractableUIManager.Instance.ShowInteractable(interactable);
            }
            else
            {
                InteractableUIManager.Instance.HideInteractable(interactable);
            }
        }
        interactedObjects.RemoveWhere(interactable =>
            interactable != null &&
            interactable.isOneTimeInteraction &&
            !colliders.Any(c => c.GetComponent<Interactable>() == interactable)

        );

    }


    void UpdateAnimator(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
            animator.SetBool("isTalking", isTalking);
            animator.SetBool("isInteracting", isInteracting);
        }
    }

    public void StartInteracting()
    {
        isInteracting = true;
        CameraManager.Instance.ZoomIn();
        UpdateAnimator(moveDirection.magnitude);
    }

    public void EndInteracting()
    {
        isInteracting = false;
        CameraManager.Instance.ZoomOut();
        UpdateAnimator(moveDirection.magnitude);
    }

    public void StartTalking()
    {
        isTalking = true;
        CameraManager.Instance.ZoomIn();
        UpdateAnimator(moveDirection.magnitude);
    }
    public void EndTalking()
    {
        isTalking = false;
        CameraManager.Instance.ZoomOut();
        UpdateAnimator(moveDirection.magnitude);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
}

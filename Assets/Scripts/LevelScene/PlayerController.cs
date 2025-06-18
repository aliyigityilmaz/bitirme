using UnityEngine;
using System.Linq;
using System.Collections.Generic; // En yakýn nesneyi bulmak için

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float moveSpeed = 5f;
    public float interactionRadius = 2f; // Etkileþim yarýçapý

    private Vector3 moveDirection;
    private HashSet<Interactable> interactedObjects = new HashSet<Interactable>();

    private Animator animator; // Animator bileþeni

    public bool isTalking = false;
    private bool isInteracting = false;

    private float stepTimer = 0f;
    public float stepInterval = 0.4f; // Adým sesleri arasýndaki süre
    private int stepIndex = 0; // 0: Step1, 1: Step2


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Singleton deseni için, baþka bir örnek varsa yok et
        }
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Animator bileþenini al
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
            UpdateAnimator(0f); // Hareket yokken hýz 0 olmalý
            return;
        }
        if (isInteracting) {
            animator.SetBool("isInteracting", true);
            UpdateAnimator(0f); // Hareket yokken hýz 0 olmalý
            return;
        }
        HandleMovement();
        HandleInteraction();
        UpdateInteractableObjects();

        UpdateAnimator(moveDirection.magnitude);
        HandleFootsteps();

    }
    void HandleFootsteps()
    {
        // Hareket ediyorsa ve yürümeye devam ediyorsa
        if (moveDirection.magnitude > 0.1f && !isTalking && !isInteracting)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                stepTimer = stepInterval;

                // Sýradaki adým sesini çal
                if (AudioManager.Instance != null)
                {
                    int clipIndex = (stepIndex % 2 == 0) ? GetSFXIndexByName("Step1") : GetSFXIndexByName("Step2");
                    if (clipIndex != -1)
                    {
                        AudioManager.Instance.PlaySFX(clipIndex);
                    }
                    stepIndex++;
                }
            }
        }
        else
        {
            stepTimer = 0f; // Yürümüyorsa sýfýrla
        }
    }
    int GetSFXIndexByName(string name)
    {
        for (int i = 0; i < AudioManager.Instance.sfxClips.Length; i++)
        {
            if (AudioManager.Instance.sfxClips[i] != null && AudioManager.Instance.sfxClips[i].name == name)
                return i;
        }
        return -1;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        if (input.magnitude >= 0.1f)
        {
            Transform cam = Camera.main.transform;

            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * input.z + camRight * input.x;

            // Akýcý dönüþ
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float rotateSpeed = 10f; // Daha yavaþ veya hýzlý döndürmek için bu deðeri artýr/azalt
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            // Hareket
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }


    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Eðer UI'da aktif bir seçim varsa onunla etkileþime geç
            if (InteractableUISelector.Instance != null)
            {
                InteractableUISelector.Instance.TriggerSelected();
                return;
            }

            // Aksi halde fiziksel olarak yakýndakiyle etkileþime geç
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
            if (interactable != null && !interactedObjects.Contains(interactable)) // Daha önce etkileþime girilmemiþse
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

using UnityEngine;
using System.Linq;
using System.Collections.Generic; // En yak�n nesneyi bulmak i�in

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactionRadius = 2f; // Etkile�im yar��ap�

    private Vector3 moveDirection;
    private HashSet<Interactable> interactedObjects = new HashSet<Interactable>();

    public bool isTalking = false;
    void Update()
    {
        if (isTalking) return;
        HandleMovement();
        HandleInteraction();
        UpdateInteractableObjects();
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
        if (Input.GetKeyDown(KeyCode.E)) // 'E' tu�una bas�ld���nda
        {
            Interactable nearestInteractable = FindNearestInteractable();
            if (nearestInteractable != null && !interactedObjects.Contains(nearestInteractable))
            {
                nearestInteractable.Interact(); // En yak�n objeyle etkile�im
                interactedObjects.Add(nearestInteractable); // Objeyi i�aretle, tekrar etkile�ime girmesin
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
        // E�er oyuncu etkile�im alan�ndan ��kt�ysa, tekrar etkile�ime girebilir
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        interactedObjects.RemoveWhere(interactable => !colliders.Any(c => c.GetComponent<Interactable>() == interactable));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
}

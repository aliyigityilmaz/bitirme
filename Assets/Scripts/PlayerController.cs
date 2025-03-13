using UnityEngine;
using System.Linq;
using System.Collections.Generic; // En yakýn nesneyi bulmak için

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactionRadius = 2f; // Etkileþim yarýçapý

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

        // Isometric hareket için eksenleri dönüþtür
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 isoDirection = new Vector3(moveDirection.x - moveDirection.z, 0, moveDirection.x + moveDirection.z);

        if (isoDirection != Vector3.zero)
        {
            transform.forward = isoDirection; // Karakterin hareket yönüne bakmasýný saðlar
        }

        transform.position += isoDirection * moveSpeed * Time.deltaTime;
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 'E' tuþuna basýldýðýnda
        {
            Interactable nearestInteractable = FindNearestInteractable();
            if (nearestInteractable != null && !interactedObjects.Contains(nearestInteractable))
            {
                nearestInteractable.Interact(); // En yakýn objeyle etkileþim
                interactedObjects.Add(nearestInteractable); // Objeyi iþaretle, tekrar etkileþime girmesin
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
        // Eðer oyuncu etkileþim alanýndan çýktýysa, tekrar etkileþime girebilir
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        interactedObjects.RemoveWhere(interactable => !colliders.Any(c => c.GetComponent<Interactable>() == interactable));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
}

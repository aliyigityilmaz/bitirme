using UnityEngine;
using System.Linq; // En yak�n nesneyi bulmak i�in

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactionRadius = 2f; // Etkile�im yar��ap�

    private Vector3 moveDirection;

    void Update()
    {
        HandleMovement();
        HandleInteraction();
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
            if (nearestInteractable != null)
            {
                nearestInteractable.Interact(); // En yak�n objeyle etkile�im
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
            if (interactable != null)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
}

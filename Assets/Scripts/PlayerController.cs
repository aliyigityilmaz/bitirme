using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 moveDirection;

    void Update()
    {
        HandleMovement();
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
}

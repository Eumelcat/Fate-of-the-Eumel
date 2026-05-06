using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed in units per second.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- Input Handling ---
        // For legacy Input Manager:
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Create movement vector
        movement = new Vector2(moveX, moveY);

        // Normalize to prevent faster diagonal movement
        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // --- Physics-based movement ---
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
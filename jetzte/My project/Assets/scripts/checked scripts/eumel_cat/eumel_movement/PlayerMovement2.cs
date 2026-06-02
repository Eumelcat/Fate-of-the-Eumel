using UnityEngine;
using UnityEngine.InputSystem;

// kümmert sich um eingabe, bewegung und lauf animation des spielers
public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 input;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerCombat combat;

    private bool canMove = true;

    private void Awake()
    {
        // alles auf dem gleichen objekt
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        // bewegung immer in fixedupdate damit sie nicht von der framerate abhängt
        Move();
    }

    private void HandleInput()
    {
        // wenn bewegung gesperrt ist input ignorieren
        if (!canMove)
        {
            input = Vector2.zero;
            return;
        }

        input = Vector2.zero;

        // wasd manuell abfragen
        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;

        // diagonal normalisieren damit man nicht schneller wird als bei gerader bewegung
        input = input.normalized;

        // richtung an combat weitergeben damit angriff in die richtige richtung geht
        combat?.SetMoveDirection(input);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleAnimation()
    {
        // während angriff soll laufanimation nicht überschreiben was grad abspielt
        if (combat != null && combat.IsAttacking())
            return;

        bool isMoving = input != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            // moveX moveY steuern in welche richtung die laufanimation zeigt
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
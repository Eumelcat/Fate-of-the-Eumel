using UnityEngine;

// slime ki erkennt eumel verfolgt und reagiert auf knockback
public class Slime_Movement : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 6f; // ab dieser distanz fängt der slime an zu verfolgen

    [Header("Vision")]
    public LayerMask obstacleLayer; // wände etc. die die sichtlinie blockieren können

    [HideInInspector] public Vector2 movement;
    [HideInInspector] public Vector2 lastDirection = Vector2.right; // letzte bewegungsrichtung für animation

    private EnemyCombat combat;
    private float knockbackTimer = 0f; // solange > 0 ist der slime im knockback

    void Start()
    {
        combat = GetComponent<EnemyCombat>();

        // spieler automatisch finden falls nicht im inspector zugewiesen
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // wird von enemyhealth aufgerufen wenn der slime getroffen wird
    public void ApplyKnockback(float duration)
    {
        knockbackTimer = duration;
    }

    void Update()
    {
        // toter slime bewegt sich nicht mehr
        if (combat != null && combat.IsDead) return;

        knockbackTimer -= Time.deltaTime;

        // während knockback bewegung einfrieren und warten bis timer abgelaufenn
        if (knockbackTimer > 0f)
        {
            movement = Vector2.zero;
            UpdateAnimator();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && CanSeePlayer())
        {
            // wenn nah genug zum angreifen dnn stehenbleiben sonst verfolgen
            if (combat == null || distance > combat.attackRange)
                ChasePlayer();
            else
                StopMoving();
        }
        else
        {
            // spieler außer reichweite oder hinter hindernis
            StopMoving();
        }

        UpdateAnimator();
    }

    bool CanSeePlayer()
    {
        // raycast zwischen slime und spieler trifft er ein hindernis ist die sicht blockiert
        Vector2 dir = (player.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleLayer);
        return hit.collider == null; // kein treffer ist freie sicht
    }

    void ChasePlayer()
    {
        movement = (player.position - transform.position).normalized;

        // letzte richtung merken damit animation nicht auf 0I0 springt
        if (movement.magnitude > 0.1f)
            lastDirection = movement;
    }

    void StopMoving()
    {
        movement = Vector2.zero;
    }
    
    void FixedUpdate()
    {
        // während knockback velocity auf null halten
        if (knockbackTimer > 0f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * moveSpeed;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        // x achse muss invertiert werden weil die sprites gespiegelt sind
        animator.SetFloat("MoveX", -movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }
}
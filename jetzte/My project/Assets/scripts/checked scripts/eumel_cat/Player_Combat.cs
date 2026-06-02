using UnityEngine;
using UnityEngine.InputSystem;

// alles was mit kämpfen zu tun hat: richtung, hitbox, animation, sound
public class PlayerCombat : MonoBehaviour
{
    [Header("Combat")]
    public int damage = 1;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.8f;

    private float lastAttackTime;
    private bool isAttacking;

    [Header("Target")]
    public LayerMask enemyLayer;

    [Header("Animation")]
    public Animator animator;

    [Header("Movement Reference")]
    public PlayerMovement2 movement;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip hitClip;

    [Header("Direction")]
    public Vector2 lastMoveDirection = Vector2.down; // fallback spieler schaut nach unten

    void Update()
    {
        // h zum angreifen
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
            TryAttack();
    }

    void TryAttack()
    {
        // wenn attacke schon läuft oder cooldown noch da ist dann kein weiterer attack
        if (isAttacking) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        isAttacking = true;

        movement?.SetCanMove(false); // bewegung während angriff sperren

        Vector2 attackDir = lastMoveDirection;

        // falls keine richtung bekannt ist dann einfach nach unten schlagen
        if (attackDir.sqrMagnitude < 0.01f)
            attackDir = Vector2.down;

        // hitbox mittig vor dem spieler platzieren
        Vector2 hitboxCenter = (Vector2)transform.position + attackDir * (attackRange * 0.5f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            hitboxCenter,
            attackRange * 0.5f,
            enemyLayer
        );

        // passenden animationstrigger aus der richtung ableiten
        string trigger;
        if (Mathf.Abs(attackDir.x) > Mathf.Abs(attackDir.y))
            trigger = attackDir.x > 0 ? "Attack_Right" : "Attack_Left";
        else
            trigger = attackDir.y > 0 ? "Attack_Up" : "Attack_Down";

        if (animator != null)
        {
            // alle trigger erst zurücksetzen sonst kann der animator hängen bleiben
            animator.ResetTrigger("Attack_Right");
            animator.ResetTrigger("Attack_Left");
            animator.ResetTrigger("Attack_Up");
            animator.ResetTrigger("Attack_Down");

            animator.SetTrigger(trigger);
        }

        if (audioSource != null && attackClip != null)
            audioSource.PlayOneShot(attackClip);

        foreach (Collider2D enemy in hits)
        {
            EnemyHealth hp = enemy.GetComponentInParent<EnemyHealth>();

            if (hp != null)
            {
                // knockback-richtung weg vom spieler
                Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                hp.TakeDamage(damage, knockDir);

                if (audioSource != null && hitClip != null)
                    audioSource.PlayOneShot(hitClip);
            }
        }

        // angriff so lange sperren wie die animation dauert
        Invoke(nameof(ResetAttack), GetAttackLength());
    }

    void ResetAttack()
    {
        isAttacking = false;
        movement?.SetCanMove(true);
    }

    float GetAttackLength()
    {
        if (animator == null) return 0.5f;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // fallback falls state.length 0 zurückgibt
        return state.length > 0 ? state.length : 0.5f;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.01f)
            lastMoveDirection = dir.normalized;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
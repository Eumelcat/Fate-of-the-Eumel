using UnityEngine;
using System.Collections;

// angreifen treffer kassieren sterben
public class EnemyCombat : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer sr;

    [Header("Combat")]
    public float attackRange = 1.5f;  // attack range
    public float attackCooldown = 1f; // cooldown
    public int damage = 1;            // schaden 

    private float lastAttackTime;     // wann hat er zuletzt angegriffen

    public bool IsDead { get; private set; } // nur intern setzbar
    private bool isAttacking;                // verhindert dass er mehrfach gleichzeitig angreift

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip hitClip;
    public AudioClip deathClip;

    void Update()
    {
        if (IsDead || player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        Debug.Log($"isAttacking: {isAttacking}, Cooldown verbleibend: {Mathf.Max(0, lastAttackTime + attackCooldown - Time.time):F2}, Distanz: {distance:F2}");

        if (distance <= attackRange)
            TryAttack();
    }

    void TryAttack()
    {
        if (isAttacking) return;
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(AttackTimeout());

        if (audioSource != null && attackClip != null)
            audioSource.PlayOneShot(attackClip);

        // Alle alten Trigger leeren bevor neue gesetzt werden
        animator.ResetTrigger("Attack_Up");
        animator.ResetTrigger("Attack_Down");
        animator.ResetTrigger("Attack_Right");
        animator.ResetTrigger("Hit");

        Vector2 dir = (player.position - transform.position).normalized;

        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            if (dir.y > 0)
                animator.SetTrigger("Attack_Up");
            else
                animator.SetTrigger("Attack_Down");
        }
        else
        {
            sr.flipX = dir.x < 0;
            animator.SetTrigger("Attack_Right");
        }
    }

    IEnumerator AttackTimeout()
    {
        yield return new WaitForSeconds(attackCooldown + 1f);
        isAttacking = false;
    }

    // wird von enemyhealth aufgerufen wenn der slime getroffen wurde
    public void TriggerHit()
    {
        isAttacking = false;

        if (animator != null)
            animator.SetTrigger("Hit");
    }

    // wird per animation event aufgerufen
    public void DealDamage()
    {
        // toter slime soll keinen schaden mehr machen
        if (IsDead || player == null) return;

        if (audioSource != null && hitClip != null)
            audioSource.PlayOneShot(hitClip);

        Player_Health hp = player.GetComponent<Player_Health>();
        if (hp != null)
            hp.ChangeHealth(damage);
    }

    // wird am ende der angriffsanimation per animation event aufgerufen

    public void EndAttack()
    {
        Debug.Log("EndAttack aufgerufen!");
        isAttacking = false;
    }

    public void Die()
    {
        if (IsDead) return;

        IsDead = true;

        if (audioSource != null && deathClip != null)
            audioSource.PlayOneShot(deathClip);

        // is dead aniamtion als bool
        animator.SetBool("IsDead", true);
    }
}
using UnityEngine;
using System.Collections;

// verwaltet hp, schaden reaktion und tod des slimes
public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Knockback")]
    public Rigidbody2D rb;
    public float knockbackForce = 8f;

    private EnemyCombat combat;

    void Start()
    {
        currentHealth = maxHealth;
        combat = GetComponent<EnemyCombat>();

        // falls rb im inspector nicht zugewiesen wurde, selber zuweisen
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        // toter slime bekommt keinen schaden
        if (combat != null && combat.IsDead) return;

        currentHealth -= damage;

        // knockback nur wenn rigidbody vorhanden
        if (rb != null)
            StartCoroutine(KnockbackRoutine(hitDir));

        // trefferanimation auslösen
        if (combat != null)
            combat.TriggerHit();

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator KnockbackRoutine(Vector2 hitDir)
    {
        // bewegung des gegners kurz einfrieren damit knockback nicht gebremst wird
        Slime_Movement slimeMove = GetComponent<Slime_Movement>();
        if (slimeMove != null)
            slimeMove.ApplyKnockback(0.3f);

        // velocity erst nullen, dann kraft draufgeben
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDir.normalized * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);

        // nach knockback wieder stoppen damit er nicht ewig weiterschlittert
        rb.linearVelocity = Vector2.zero;
    }

    void Die()
    {
        // sterbeanimation etc über enemycombat abhandeln
        if (combat != null)
            combat.Die();

        // kleines delay damit die animation noch abgespielt bevor objekt weg ist
        Destroy(gameObject, 0.5f);
    }
}
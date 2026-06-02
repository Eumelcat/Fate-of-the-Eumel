using UnityEngine;

// kümmert sich um leben, schaden und tod des spielers
public class Player_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public SpriteRenderer playerSr;
    public PlayerMovement2 playerMovement;

    public GameOver gameOverManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageClip;
    public AudioClip deathClip;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        // falls im inspector vergessen wurde den audiosource zuzuweisen, selber zuweisen
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void ChangeHealth(int amount)
    {
        // toter spieler kriegt keinen schaden mehr
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (audioSource != null && damageClip != null)
            audioSource.PlayOneShot(damageClip);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0; // damit nichts unter 0 gehen kann

        Debug.Log("Player dead");

        if (audioSource != null && deathClip != null)
            audioSource.PlayOneShot(deathClip);

        // spieler unsichtbar machen und bewegung sperren
        if (playerSr != null)
            playerSr.enabled = false;

        if (playerMovement != null)
            playerMovement.enabled = false;

        // game over auslösen wenn nicht zugewiesen gibts einen fehler im log
        if (gameOverManager != null)
            gameOverManager.TriggerGameOver();
        else
            Debug.LogError("GameOverManager nicht zugewiesen!");
    }
}
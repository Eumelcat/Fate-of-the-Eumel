using UnityEngine;
using UnityEngine.UI;

// zeigt die herzen der spielerhp im ui an
public class Health_Display : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public Player_Health player_Health;

    private int lastHealth = -1;
    private int lastMaxHealth = -1; // -1 damit beim ersten frame immer ein update passiert

    void Start()
    {
        UpdateHearts();
    }

    void Update()
    {
        if (player_Health == null) return;

        // nur neu zeichnen wenn sich was verändert hat
        if (player_Health.currentHealth != lastHealth ||
            player_Health.maxHealth != lastMaxHealth)
        {
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        int maxHealth = player_Health.maxHealth;
        int health = Mathf.Clamp(player_Health.currentHealth, 0, maxHealth);

        lastHealth = health;
        lastMaxHealth = maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
                hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            }
            else
            {
                // herz-slot existiert im array aber wird grad nicht gebraucht
                hearts[i].enabled = false;
            }
        }
    }
}
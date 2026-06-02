using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverScreen;

    [Header("Audio")]
    public AudioSource musicSource;        // normale Spielmusik
    public AudioSource rainSource;         // regen sound
    public AudioSource gameOverMusic;      // game over musik

    private bool isGameOver = false;

    // wird aufgerufen wenn spieler stirbt
    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Spiel stoppen
        Time.timeScale = 0f;

        // Sounds ausschalten
        if (musicSource != null)
            musicSource.Stop();

        if (rainSource != null)
            rainSource.Stop();

        // Game Over Screen zeigen
        gameOverScreen.SetActive(true);

        // Game Over Musik starten
        if (gameOverMusic != null)
            gameOverMusic.Play();
    }

    // Restart Game
    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Openworld");
    }

    // zurück ins Menu
    public void ExitButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start_Scene");
    }
}
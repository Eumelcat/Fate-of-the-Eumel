using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField] GameObject pauseMenu;


    void Update()
    {
        // prüft jedes Frame ob die escTaste gedrückt wurde
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // wenn das Menü gerade sichtbar ist
            if (pauseMenu.activeSelf)
            {
                Resume(); // spiel weiterlaufen lassen
            }
            else
            {
                Pause(); // spiel pausieren und Menü öffnen
            }
        }
    }

    // wird aufgerufen wenn man auf pause button drückt
    public void Pause()
    {
        pauseMenu.SetActive(true); // menu einblenden
        Time.timeScale = 0; // spiel anhalten, zeit auf 0 setzen
    }

    
    
    // geht zurück zum hauptmenü
    public void Home()
    {
        SceneManager.LoadScene("Start_Scene"); // start szene laden
        Time.timeScale = 1; // zeit wieder auf normal sonst bleibt alles eingefroren
    }

    // weiterspielen nach der pause
    public void Resume()
    {
        pauseMenu.SetActive(false); // menu ausblenden
        Time.timeScale = 1; // spiel läuft wieder normal
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject quitPanel; // das Panel das aufpoppt wenn man auf Quit drückt

    [Header("Sound")]
    [SerializeField] private AudioSource musicSource;      // AudioSource für die Hintergrundmusik
    [SerializeField] private AudioSource sfxSource;        // extra AudioSource nur für Klick Sounds
    [SerializeField] private AudioClip backgroundMusic;    // die Musikdatei die in Schleife läuft
    [SerializeField] private AudioClip buttonClickSound;   // der Klick Sound für alle Buttons
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;  // Lautstärke der Musik
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 1.0f;    // Lautstärke der Klick Sounds

    private void Start()
    {
        // Musik starten sobald das menu lädt
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic; // Musikdatei zuweisen
            musicSource.loop = true;            // in Schleife abspielen
            musicSource.volume = musicVolume;   // Lautstärke setzen
            musicSource.Play();                 // starten
        }

        // SFX Lautstärke setzen
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    // wird vom Play-Button aufgerufen
    public void PlayGame()
    {
        PlayClickSound();                  // Klick Sound abspielen
        SceneManager.LoadSceneAsync(4);    // Szene laden
    }

    // wird vom Quit Button im menu aufgerufen
    public void QuitGame()
    {
        PlayClickSound();              // klick Sound abspielen
        quitPanel.SetActive(true);     // Bestätigungs Panel anzeigen
    }

    // Nein Button
    public void CancelQuit()
    {
        PlayClickSound();              // Klick Sound abspielen
        quitPanel.SetActive(false);    // Panel wieder verstecken
    }

    // Ja Button
    public void ConfirmQuit()
    {
        PlayClickSound();          // Klick Sound abspielen
        Application.Quit();        // Spiel beenden

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // im Editor einfach den PlayMode stoppen
#endif
    }

    // public damit EventTrigger sie aufrufen kann
    public void PlayClickSound()
    {
        if (sfxSource != null && buttonClickSound != null)
            sfxSource.PlayOneShot(buttonClickSound); // PlayOneShot damit sich Klicks nicht gegenseitig abwürgen
    }
}
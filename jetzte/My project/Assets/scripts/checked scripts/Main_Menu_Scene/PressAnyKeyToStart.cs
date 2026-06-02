using UnityEngine;                    
using UnityEngine.SceneManagement;   
using UnityEngine.InputSystem;       
using System.Collections;         

public class PressAnyKeyToStart : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;           // ein schwarzes Panel im Canvas, dessen Alpha wir hochdrehen
    [SerializeField] private string sceneName = "Start_Scene"; // der Name der Szene die geladen wird
    [SerializeField] private float transitionDuration = 3f;    // länge von fade

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;          // die AudioSource auf dem Gameobject
    [SerializeField] private AudioClip keyPressSound;          // der Sound der abgespielt wird wenn man eine Taste drückt

    private bool keyPressed = false;          // damit der Fade nicht zweimal startet wenn man schnell drückt

    private void Start()
    {
        if (fadeCanvas != null) fadeCanvas.alpha = 0f; // beim Start ist das schwarze Panel komplett unsichtbar
    }

    private void Update()
    {
        // wenn taste gedrückt wird, Fade starten
        if (!keyPressed && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            PlayKeyPressSound(); // erstmal den Sound abspielen
            StartCoroutine(StartTransition()); // dann den Fade starten
        }
    }

    private void PlayKeyPressSound()
    {
        // nur abspielen wenn auch wirklich eine AudioSource und ein Clip zugewiesen sind
        if (audioSource != null && keyPressSound != null)
            audioSource.PlayOneShot(keyPressSound); // PlayOneShot damit der Sound nicht abgehackt wird
    }

    private IEnumerator StartTransition()
    {
        keyPressed = true; // sperren damit die Coroutine nicht nochmal startet

        float elapsed = 0f; // zählt wie lange der Fade schon läuft

        while (elapsed < transitionDuration) // so lange bis der Fade fertig ist
        {
            elapsed += Time.deltaTime; // Zeit hochzählen

            // smooth macht weich
            float smooth = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            if (fadeCanvas != null)
                fadeCanvas.alpha = smooth; // schwarzes Panel wird langsam sichtbarer, Bild wird dunkel

            yield return null; // einen Frame warten, dann weitermachen
        }

        SceneManager.LoadScene(sceneName); // wenn der Fade durch ist, Szene laden
    }
}
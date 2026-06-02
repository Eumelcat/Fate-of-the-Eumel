using UnityEngine;

public class UISound : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;       // AudioSource für die Klick-Sounds
    [SerializeField] private AudioClip buttonClickSound;  // der Klick-Sound

    public void PlayClickSound()
    {
        if (sfxSource != null && buttonClickSound != null)
            sfxSource.PlayOneShot(buttonClickSound); // PlayOneShot damit sich Klicks nicht gegenseitig abwürgen
    }
}
using UnityEngine;
using UnityEngine.UI;

// spielt einen klick sound ab wenn dieser button gedrückt wird
[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource source;

    void Awake()
    {
        // schauen ob die main camera eine audiosource hat
        source = Camera.main.GetComponent<AudioSource>();

        // falls nicht dann eine auf diesem objekt erstellen als fallback
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();

        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        // beide checkskein clip oder keine source, dann gar nichts
        if (clickSound == null) return;
        if (source == null) return;

        source.PlayOneShot(clickSound);
    }
}
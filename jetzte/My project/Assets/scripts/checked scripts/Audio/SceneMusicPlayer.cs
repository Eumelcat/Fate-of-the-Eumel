using UnityEngine;

// spielt die musik tracks der szene nacheinander ab und fängt nach dem letzten wieder von vorne an
public class SceneMusicPlayer : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip[] musicClips;

    private int index; // welcher track als nächstes dran ist

    void Start()
    {
        // audiosource selbert holen falls im inspector vergessen wurde
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        PlayNext();
    }

    void PlayNext()
    {
        // wenn kein array oder leer dann einfach nichts tun
        if (musicClips == null || musicClips.Length == 0) return;

        musicSource.clip = musicClips[index];
        musicSource.loop = false; // loop aus, weil selbst den nächsten track starten
        musicSource.Play();

        // index schon hier hochzählen damit beim nächsten aufruf der richtige track kommt
        index = (index + 1) % musicClips.Length; // % sorgt dafür dass nach dem letzten wieder index 0 kommt

        // playnext nochmal aufrufen wenn der track fertig ist
        Invoke(nameof(PlayNext), musicSource.clip.length);
    }
}
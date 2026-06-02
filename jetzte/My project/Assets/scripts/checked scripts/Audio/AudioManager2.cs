using UnityEngine;
using System.Collections;

// regen läuft dauerhaft undMusik Playlist separat
public class AudioManager2 : MonoBehaviour
{
    [Header("Rain")]
    public AudioSource rainSource;

    [Header("Music Playlist")]
    public AudioSource musicSource;
    public AudioClip[] musicClips;

    [Header("Pause zwischen Songs")]
    public float minPause = 5f;
    public float maxPause = 15f;

    private int index = 0;

    void Start()
    {
        //  ein regen clip der durchgehen läuft
        rainSource.loop = true;
        rainSource.Play();

        // Musik starten
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        while (true)
        {
            // Pause zwischen Songs
            yield return new WaitForSeconds(Random.Range(minPause, maxPause));

            // aktuellen Song holen
            AudioClip clip = musicClips[index];

            musicSource.clip = clip;
            musicSource.Play();

            // warten bis Song fertig ist
            yield return new WaitForSeconds(clip.length);

            musicSource.Stop();

            // nächster Song in Playlist
            index++;

            if (index >= musicClips.Length)
                index = 0;
        }
    }
}
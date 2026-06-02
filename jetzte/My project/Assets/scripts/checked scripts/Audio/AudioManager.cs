using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// zentraler audiomanager – bleibt über alle szenen bestehen
// kümmert sich um musik, sfx, uisounds und lautstärke
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Sources")]
    public AudioSource musicSource; // nur für hintergrundmusik
    public AudioSource sfxSource;   // nur für soundeffekte

    [Header("UI")]
    public AudioClip uiClickSound;  // wird automatisch an alle buttons gehängt

    [Header("Scene Music")]
    public List<SceneMusic> sceneMusicList; // im inspector befüllen szenenname und clips

    private Dictionary<string, AudioClip[]> musicMap; // schneller zugriff szenenname clips
    private string currentScene;
    private int musicIndex; // welcher track in der playlist gerade dran ist

    private const string VOLUME_PARAM = "MasterVol"; // muss exakt so heißen wie der parameter im mixer

    void Awake()
    {
        // singleton pattern nur eine instanz erlaubt doppelte zerstört
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // bleibt bei szenenwechsel

        // früh warnen falls wichtige referenzen fehlen
        if (mixer == null)
            Debug.LogError("AudioMixer fehlt im AudioManager!");

        if (musicSource == null || sfxSource == null)
            Debug.LogError("AudioSources fehlen im AudioManager!");

        // list in dictionary umwandeln damit später schnell nach szenenname suchen können
        musicMap = new Dictionary<string, AudioClip[]>();

        foreach (var sm in sceneMusicList)
        {
            // doppelte szeneneinträge überspringen
            if (!musicMap.ContainsKey(sm.sceneName))
                musicMap.Add(sm.sceneName, sm.clips);
        }

        // auf szenenwechsel reagieren
        SceneManager.sceneLoaded += OnSceneLoaded;

        // gespeicherte lautstärke direkt beim start laden
        LoadVolume();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;

        // passende musik für neue szene starten
        PlaySceneMusic(currentScene);

        // alle buttons in der szene mit klicksound versehen
        HookUIButtonSounds();
    }

    public void PlaySceneMusic(string sceneName)
    {
        // wenn keine musik für diese szene definiert, dann nichts\
        if (!musicMap.ContainsKey(sceneName)) return;

        AudioClip[] clips = musicMap[sceneName];
        if (clips == null || clips.Length == 0) return;

        // playlist von vorne starten
        musicIndex = 0;
        PlayMusic(clips[musicIndex]);
    }

    void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = false; // loop aus playlist-coroutine übernimmt das weiterspielen
        musicSource.Play();

        // alte playlist coroutine stoppen damit nicht zwei gleichzeitig laufen
        StopAllCoroutines();
        StartCoroutine(PlayPlaylist());
    }

    IEnumerator PlayPlaylist()
    {
        // warten bis der aktuelle track fertig ist
        yield return new WaitWhile(() => musicSource.isPlaying);

        // szene könnte gewechselt haben
        if (!musicMap.ContainsKey(currentScene)) yield break;

        AudioClip[] clips = musicMap[currentScene];
        if (clips.Length == 0) yield break;

        // nächsten track laden nach dem letzten wieder von vorne
        musicIndex = (musicIndex + 1) % clips.Length;

        PlayMusic(clips[musicIndex]);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        // playoneshot damit sich sounds überlagern können ohne sich gegenseitig abzuschneiden
        sfxSource.PlayOneShot(clip);
    }

    public void PlayUIClick()
    {
        PlaySFX(uiClickSound);
    }

    void HookUIButtonSounds()
    {
        // alle buttons in der aktuellen szene finden
        var buttons = Object.FindObjectsByType<UnityEngine.UI.Button>(
            FindObjectsSortMode.None
        );

        foreach (var b in buttons)
        {
            // erst entfernen dann neu hinzufügen
            // falls hookuibuttonsounds mehrfach aufgerufen wird
            b.onClick.RemoveListener(PlayUIClick);
            b.onClick.AddListener(PlayUIClick);
        }
    }

    public void SetVolume(float value)
    {
        if (mixer == null) return;

        // clamp auf 0.0001f damit log10 nicht gegen unendlich geht
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;

        mixer.SetFloat(VOLUME_PARAM, dB);

        // wert speichern damit er beim nächsten spielstart wieder geladen werden kann
        PlayerPrefs.SetFloat("volume", value);
    }

    void LoadVolume()
    {
        // gespeicherten wert holen falls nichts da ist dann 1f 
        float v = PlayerPrefs.GetFloat("volume", 1f);
        SetVolume(v);
    }
}

// wird im inspector als liste befüllt 
[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public AudioClip[] clips;
}
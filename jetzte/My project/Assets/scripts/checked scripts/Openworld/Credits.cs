using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EndScreenClean : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup winText;
    public RectTransform credits;

    [Header("Credits")]
    public float scrollSpeed = 50f;
    public float endY = 1000f;

    [Header("Scene")]
    public string nextScene;
    public float fadeDuration = 1.5f;

    [Header("Audio")]
    public AudioClip music;
    public float musicVolume = 0.8f;

    private AudioSource audioSource;
    private Image fadeImage;
    private bool scrolling = false;

    void Start()
    {
        SetupFade();
        SetupAudio();
        StartCoroutine(StartSequence());
    }

    void SetupFade()
    {
        GameObject obj = new GameObject("Fade");
        obj.transform.SetParent(transform, false);

        fadeImage = obj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void SetupAudio()
    {
        if (music == null) return;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeAudio(0f, musicVolume, 2f));
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);

        // Win Text ausblenden
        while (winText.alpha > 0)
        {
            winText.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        winText.gameObject.SetActive(false);
        scrolling = true;
    }

    void Update()
    {
        if (!scrolling) return;

        credits.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (credits.anchoredPosition.y >= endY)
        {
            scrolling = false;
            StartCoroutine(EndSequence());
        }
    }

    IEnumerator EndSequence()
    {
        // Musik ausfaden + Screen einfaden gleichzeitig
        StartCoroutine(FadeAudio(audioSource.volume, 0f, 2f));

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }

    IEnumerator FadeAudio(float from, float to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        audioSource.volume = to;
    }
}
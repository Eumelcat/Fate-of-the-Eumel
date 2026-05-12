using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager2 : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;
    [SerializeField] private float startDelay = 2f;

    private float shownProgress = 0f;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(startDelay);

        AsyncOperation load = SceneManager.LoadSceneAsync("Openworld");
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            float target = Mathf.Clamp01(load.progress / 0.9f);

            // „echtes“ Ladegefühl: manchmal kurz hängen bleiben
            bool feelsStuck = Random.value < 0.15f;

            float speed = feelsStuck ? 0f : Random.Range(0.3f, 0.7f);

            shownProgress = Mathf.MoveTowards(shownProgress,target,Time.deltaTime * speed);

            progressBar.value = shownProgress;
            loadingText.text = $"Loading... {Mathf.RoundToInt(shownProgress * 100)}%";

            // kleine natürliche Pause (wirkt wie „kurz nachdenken“)
            if (feelsStuck)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
            }

            // fertig → Szene wechseln
            if (shownProgress >= 0.99f && load.progress >= 0.9f)
            {
                loadingText.text = "Loading... 100%";
                yield return new WaitForSeconds(1f);
                load.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
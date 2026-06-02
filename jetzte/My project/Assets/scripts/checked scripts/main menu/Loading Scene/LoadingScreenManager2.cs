using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ladebildschirm mit animiertem fortschrittsbalken für "openworld" szene
public class LoadingScreenManager2 : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;
    [SerializeField] private float startDelay = 2f; // kurze pause vor ladestart

    private float shownProgress = 0f; // angezeigter fortschritt 

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(startDelay);

        // szene laden, aber noch nicht aktivieren
        AsyncOperation load = SceneManager.LoadSceneAsync("Openworld");
        load.allowSceneActivation = false;

        while (!load.isDone)
        {
            // unity meldet max 0.9f solange allowSceneActivation = false auf 0–1 normalisieren
            float target = Mathf.Clamp01(load.progress / 0.9f);

            // zufällig kurz einfrieren
            bool feelsStuck = Random.value < 0.15f;
            float speed = feelsStuck ? 0f : Random.Range(0.3f, 0.7f);

            // angezeigten fortschritt sanft an echten wert annähern
            shownProgress = Mathf.MoveTowards(shownProgress, target, Time.deltaTime * speed);

            progressBar.value = shownProgress;
            loadingText.text = $"Loading... {Mathf.RoundToInt(shownProgress * 100)}%";

            if (feelsStuck)
                yield return new WaitForSeconds(Random.Range(0.1f, 0.4f)); // kurze pause

            // beide bedingungen erfüllt szene freischalten
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
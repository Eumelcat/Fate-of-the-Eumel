using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PressAnyKeyToStart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pressText;
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private string sceneName = "Start_Scene";

    private bool keyPressed = false;

    private string baseText = "Press Any Key To Start";
    private float timer;
    private int dotCount;

    private void Update()
    {
        AnimateDots();

        if (!keyPressed && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartCoroutine(StartTransition());
        }
    }

    private void AnimateDots()
    {
        if (pressText == null) return;

        timer += Time.deltaTime;

        if (timer >= 0.5f)
        {
            timer = 0f;
            dotCount = (dotCount + 1) % 4;
        }

        pressText.text = baseText + new string('.', dotCount);
    }

    private IEnumerator StartTransition()
    {
        keyPressed = true;

        // Fade out
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject quitPanel;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(4);
    }

    // wird vom Quit-Button im Menü aufgerufen
    public void QuitGame()
    {
        quitPanel.SetActive(true);
    }

    // "Nein" Button
    public void CancelQuit()
    {
        quitPanel.SetActive(false);
    }

    // "Ja" Button
    public void ConfirmQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
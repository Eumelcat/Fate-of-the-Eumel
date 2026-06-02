using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// spielt intro-video ab und wechselt danach zur menuszene
public class Intro_Scene_Manager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoplayer;

    private void Start()
    {
        // event registrieren wird aufgerufen wenn video zu ende ist
        videoplayer.loopPointReached += Videoplayer_loopPointReached;
    }

    private void Videoplayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("Intro_Scene_Mecces"); //mecces szene laden
    }
}

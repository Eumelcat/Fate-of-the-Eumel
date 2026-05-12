using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro_Scene_Manager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoplayer;

    private void Start()
    {
        videoplayer.loopPointReached += Videoplayer_loopPointReached;
    }

    private void Videoplayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("Intro_Scene_Mecces");
    }
}

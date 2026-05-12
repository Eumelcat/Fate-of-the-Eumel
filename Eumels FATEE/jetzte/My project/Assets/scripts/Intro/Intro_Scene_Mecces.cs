using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro_Scene_Mecces : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoplayer;

    private void Start()
    {
        videoplayer.loopPointReached += Videoplayer_loopPointReached;
    }

    private void Videoplayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("Menu_Scene");
    }
}

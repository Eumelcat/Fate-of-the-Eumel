using UnityEngine;

public class MenuScreens : MonoBehaviour
{
    public GameObject optionsScreen;
    public GameObject controlsScreen;

    // options aus, controls an
    public void OpenControls()
    {
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    // controls aus, options an
    public void BackToOptions()
    {
        controlsScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
}
using UnityEditor.Macros;
using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{
    public void SetQuality (int qualityIndex) //qualität setzen
    {
        QualitySettings.SetQualityLevel(qualityIndex); //methode zur qualitätsetzung
    }

    public void SetFullscreen (bool isFullscreen) //fullscreen setzen
    {
        Screen.fullScreen = isFullscreen; // methode zur fullscreensetzung
    }
}
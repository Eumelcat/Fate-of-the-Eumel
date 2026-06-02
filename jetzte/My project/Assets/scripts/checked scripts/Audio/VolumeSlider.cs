using UnityEngine;
using UnityEngine.UI;

// verbindet den lautstärke slider im ui mit dem audiomanager
public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        // erstmal alle alten listener weg falls noch irgendwas dranhängt
        slider.onValueChanged.RemoveAllListeners();

        // gespeicherten wert laden falls nichts gespeichert ist dann 1
        slider.value = PlayerPrefs.GetFloat("volume", 1f);

        // jetzt erst listener hinzufügen
        slider.onValueChanged.AddListener(OnChange);
    }

    void OnChange(float v)
    {
        // sicher falls der audiomanager noch nicht existiert
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.SetVolume(v);
    }
}
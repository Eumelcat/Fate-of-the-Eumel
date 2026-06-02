using UnityEngine;
using UnityEngine.UI;

// spielt einen klicksound ab wenn der button gedrückt wird
[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();

        // listener direkt in awake damit er garantiert drauf ist bevor irgendwas den button drückt
        btn.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        // kein audiomanager, kein sound
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayUIClick();
    }
}
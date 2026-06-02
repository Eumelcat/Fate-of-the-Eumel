using UnityEngine;
using TMPro; // brauch ich für textMeshPro

// zählt die zeit hoch und zeigt sie im ui an
public class Timer : MonoBehaviour
{
    public float time; // hier wird die aktuelle zeit gespeichert

    public TextMeshProUGUI timertext; // das textfeld im ui, zieh ich im inspector rein

    void Update()
    {
        time += Time.deltaTime; // zeit hochzählen, deltaTime ist die zeit seit dem letzten frame

        timertext.text = Mathf.Floor(time).ToString(); // kommazahlen abrunden und als text anzeigen
    }
}
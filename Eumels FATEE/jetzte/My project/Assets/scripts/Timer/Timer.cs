using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time;

    public TextMeshProUGUI timertext;
    void Update()
    {
        time += Time.deltaTime;
        timertext.text = Mathf.Floor(time).ToString();
    }
}

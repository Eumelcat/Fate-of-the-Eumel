using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    
    public static LoadingScreenManager Instance;
    public GameObject m_LoadingScreenObject;
    public Slider Progressbar;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    public void SwitchToScene(int id)
    {
        m_LoadingScreenObject.SetActive(true);
        Progressbar.value = 0;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

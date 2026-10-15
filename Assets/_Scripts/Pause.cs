using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PanelOnPause;
    public GameObject PanelOnGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PanelOnPause != null)
        {
            PanelOnPause.SetActive(false);
            PanelOnGame.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 && PanelOnPause != null && PanelOnPause.activeSelf == false)
        {
            PanelOnPause.SetActive(true);
            PanelOnGame.SetActive(false);
        }
    }

    public void ClickButtonPause()
    {
        Time.timeScale = 0;

    }

    public void ClickButtonContinue()
    {
        Time.timeScale = 1;
        if (PanelOnPause != null)
        {
            PanelOnPause.SetActive(false);
            PanelOnGame.SetActive(true);
        }
    }

    public void ClickButtonExit()
    {
        Time.timeScale = 1;
        if (PanelOnPause != null)
        {
            PanelOnPause.SetActive(true);
            PanelOnGame.SetActive(true);
        }
        SceneManager.LoadScene("StartScene");
    }


}

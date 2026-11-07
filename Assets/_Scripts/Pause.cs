using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject PanelOnPause;
    public GameObject PanelOnGame;
    public GameObject PanelHelpOnStart;
    private GameObject GameManag;
    private GameManager GamemanagerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        
    }
    void Start()
    {
        GameManag = GameObject.Find("GameManager");
        if (GameManag != null) { GamemanagerScript = GameManag.gameObject.GetComponent<GameManager>(); }
        if (PanelHelpOnStart != null && PlayerPrefs.GetInt("NeedShowPanelHelp", 0) == 1)
        {
            PanelHelpOnStart.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("NeedShowPanelHelp", 0);
        }
        else
        {
            if (GameManag != null) { GamemanagerScript.startingGame = true; }
        }
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
            if (PanelHelpOnStart != null && PanelHelpOnStart.activeSelf == false)
            {
                PanelOnPause.SetActive(true);
                PanelOnGame.SetActive(false);
            }
        }
    }

    public void ClickButtonPause()
    {
        Time.timeScale = 0;

    }

    public void OkeyPanelHelpOnStart()
    {
        Time.timeScale = 1;
        if (PanelHelpOnStart != null)
        {
            PanelHelpOnStart.SetActive(false);
            GamemanagerScript.startingGame = true;
        }
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

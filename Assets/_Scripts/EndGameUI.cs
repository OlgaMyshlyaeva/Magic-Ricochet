using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Управляет отображением результатов на финальном экране.
/// Обрабатывает логику вывода рекордов и навигацию по сценам.
/// </summary>
public class EndGameUI : MonoBehaviour
{
    [Header("UI Elements")]
    //public TextMeshProUGUI statusText;    // Текст "VICTORY" или "GAME OVER"
    public GameObject PanelDefeat;
    public GameObject PanelWin;
    public GameObject PanelNewRecord;
    public TextMeshProUGUI finalTimeText;
    //public TextMeshProUGUI bestTimeText;
    private SoundsMaster soundsMasterScript;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "_Scene_0";
    [SerializeField] private string menuSceneName = "StartScene";

    void Start()
    {
        
        GameObject soundsMaster = GameObject.Find("SoundMaster");
        soundsMasterScript = soundsMaster.GetComponent<SoundsMaster>();
        DisplayResults();
    }

    /// <summary>
    /// Загружает данные из PlayerPrefs и обновляет интерфейс.
    /// </summary>
    private void DisplayResults()
    {
        // Читаем время последней игры и рекорд
        float lastTime = PlayerPrefs.GetFloat("FinalTime", 0f);
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        float lastBestTime = PlayerPrefs.GetFloat("LastBestTime", 0f);

        // Проверяем, был ли это выигрыш (логика из GameManager)
        // Для этого в GameManager.GameOver добавьте: PlayerPrefs.SetInt("IsWin", isWin ? 1 : 0);
        bool isWin = PlayerPrefs.GetInt("IsWin", 0) == 1;

        //Проигрывание звука
        if (isWin==false)
        {
            int randomNumber = Random.Range(1, 6);
            switch (randomNumber)
            {
                //Звук поражения 1
                case 1: soundsMasterScript.PlaySoundSadtromboneacappella(); break;

                //Звук поражения 2
                case 2: soundsMasterScript.PlaySoundSadtrombone1(); break;

                //Звук поражения  3
                case 3: soundsMasterScript.PlaySoundSadtromboneshort(); break;

                //Звук поражения  4
                case 4: soundsMasterScript.PlaySoundSadtromboneplayback(); break;

                //Звук поражения  5
                case 5: soundsMasterScript.PlaySoundSadtrombonecartoon(); break;
            }
        }

        // 1. Устанавливаем заголовок
        if (isWin == false)
        {
            PanelDefeat.SetActive(true);
            PanelWin.SetActive(false);
        }
        else
        {
            PanelDefeat.SetActive(false);
            PanelWin.SetActive(true);
        }

            // 2. Выводим текущее время
            if (finalTimeText != null)
        {
            finalTimeText.text = string.Format("{0:00}:{1:00}.{2:00}s", Mathf.FloorToInt(lastTime / 60F), Mathf.FloorToInt(lastTime % 60F), Mathf.Min(Mathf.RoundToInt((lastTime % 60F % 1F * 100)), 99));
        }

        // 3. Выводим лучший результат
        if (lastBestTime > bestTime || lastBestTime==0f)
        {
            PanelNewRecord.SetActive(true);
            lastBestTime = bestTime;
            PlayerPrefs.SetFloat("LastBestTime", lastBestTime);
        }
        else { PanelNewRecord.SetActive(false); }


    }

    // Методы для кнопок (удобно для настройки в инспекторе)
    public void RestartGame() 
    { 
        SceneManager.LoadScene(gameSceneName); 
    }

    public void ToMainMenu() 
    { 
        SceneManager.LoadScene(menuSceneName); 
    }
}


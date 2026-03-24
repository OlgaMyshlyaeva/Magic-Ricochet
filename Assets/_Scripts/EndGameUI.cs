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
    public TextMeshProUGUI statusText;    // Текст "VICTORY" или "GAME OVER"
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI bestTimeText;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "_Scene_0";
    [SerializeField] private string menuSceneName = "StartScene";

    void Start()
    {
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

        // Проверяем, был ли это выигрыш (логика из GameManager)
        // Для этого в GameManager.GameOver добавьте: PlayerPrefs.SetInt("IsWin", isWin ? 1 : 0);
        bool isWin = PlayerPrefs.GetInt("IsWin", 0) == 1;

        // 1. Устанавливаем заголовок
        if (statusText != null)
        {
            statusText.text = isWin ? "VICTORY!" : "DEFEAT!";
            statusText.color = isWin ? Color.green : Color.red;
        }

        // 2. Выводим текущее время
        if (finalTimeText != null)
        {
            finalTimeText.text = $"Your Time: {lastTime:F2}s";
        }

        // 3. Выводим лучший результат
        if (bestTimeText != null)
        {
            if (bestTime <= 0 || bestTime >= 998f)
            {
                bestTimeText.text = "Best Time: --";
            }
            else
            {
                bestTimeText.text = $"Best Time: {bestTime:F2}s";
            }
        }
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


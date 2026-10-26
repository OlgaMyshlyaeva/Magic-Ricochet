using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Главный менеджер игры. Отвечает за таймер, условия победы и сохранение рекордов.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager S; // Singleton для доступа из других скриптов

    [Header("UI Settings")]
    public TextMeshProUGUI timerText;
    public GameObject Besttime;
    
    [Header("Game Stats")]
    public float timer = 0f;
    private bool _isGameActive = true;
    public bool startingGame = false;

<<<<<<< Updated upstream
=======
    public GameObject TextTimer;
    public TextMeshProUGUI BestTimeOnGame;

>>>>>>> Stashed changes
    void Awake() 
    { 
        if (S == null) S = this; 
    }

    private void Start()
    {
        if (BestTimeOnGame != null) { BestTimeOnGame.text = string.Format("{0:00}:{1:00}.{2:00}s", Mathf.FloorToInt(PlayerPrefs.GetFloat("BestTime") / 60F), Mathf.FloorToInt(PlayerPrefs.GetFloat("BestTime") % 60F), Mathf.Min(Mathf.RoundToInt((PlayerPrefs.GetFloat("BestTime") % 60F % 1F * 100)), 99));  }
        if (PlayerPrefs.GetFloat("BestTime") == 0f) { Besttime.SetActive(false); } else { Besttime.SetActive(true); }
    }

    void Update()
    {
        if (!_isGameActive) return;

<<<<<<< Updated upstream
        timer += Time.deltaTime;
        if (timerText != null) 
            timerText.text = "Time: " + timer.ToString("F2") + "s";
=======
        if (startingGame)
        {
            timer = Time.deltaTime + timer;
        }
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            float remainingSeconds = timer % 60F;
>>>>>>> Stashed changes

            //timerText.text = string.Format("{0:00}:{1:00}:{2:00}s", minutes, Mathf.FloorToInt(remainingSeconds), (remainingSeconds - Mathf.FloorToInt(remainingSeconds)) * 100);

            timerText.text = string.Format("{0:00}:{1:00}.{2:00}s", minutes, Mathf.FloorToInt(remainingSeconds), Mathf.Min(Mathf.RoundToInt((remainingSeconds % 1F * 100)),99));

            //timerText.text = timer.ToString("F2") + "s";

        }


        // Если все цели (кроме черных и красных) уничтожены — это ПОБЕДА
        if (CheckVictoryConditions())
        {
            GameOver(true);
        }
    }

    /// <summary>
    /// Проверяет, остались ли на сцене живые цели.
    /// </summary>
    private bool CheckVictoryConditions()
    {
        /*
        EnemyPatrol[] allEnemies = Object.FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None);
        foreach (EnemyPatrol enemy in allEnemies)
        {
            // Если нашли хоть одного НЕ черного скелета, который еще жив — играем дальше
            if (enemy.type != EnemyPatrol.EnemyType.Black && !enemy.isDead) 
                return false; 
        }
        */

        EnemyPatrol[] allEnemies = Object.FindObjectsByType<EnemyPatrol>(FindObjectsSortMode.None);

        foreach (EnemyPatrol enemy in allEnemies)
        {
            if ((!enemy.isDead && (enemy.type != EnemyPatrol.EnemyType.Black && enemy.type != EnemyPatrol.EnemyType.Red)))
            {
                return false;
                
            }
        }

        /*
        foreach (EnemyPatrol enemy in allEnemies)
        {
            // Если нашли хоть одного НЕ красного, который еще жив — играем дальше
            if ((enemy.type != EnemyPatrol.EnemyType.Red && !enemy.isDead))
                return false;
        }
        */

        // Если есть скрипты Target (вазы/ящики), проверяем и их
        Target[] allTargets = Object.FindObjectsByType<Target>(FindObjectsSortMode.None);
        foreach (Target t in allTargets)
        {
            if (!t.IsDestroyed) return false; 
        }

        return true; 
    }

    // Метод для совместимости со старыми вызовами из Target.cs
    public void OnEnemyDefeated() { /* Логика теперь в Update */ }

    public void AddBonusTime(float amount) { if (_isGameActive) timer += amount; }

    /// <summary>
    /// Завершает игру и обрабатывает сохранение рекордов.
    /// </summary>
    /// <param name="isWin">True — если победа, False — если попали в черного скелета.</param>
    public void GameOver(bool isWin)
    {
        if (!_isGameActive) return;
        _isGameActive = false;

        // 1. Сохраняем время текущего захода (для отображения в конце)
        PlayerPrefs.SetFloat("FinalTime", timer);
        // Сохраняем статус для UI (Победа или Поражение)
        PlayerPrefs.SetInt("IsWin", isWin ? 1 : 0);
        
        // 2. РЕКОРД ОБНОВЛЯЕТСЯ ТОЛЬКО ПРИ ПОБЕДЕ
        if (isWin)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime", 999f);
            if (timer < bestTime)
            {
                PlayerPrefs.SetFloat("BestTime", timer);
                Debug.Log("New Record Set: " + timer);
            }
        }
        else
        {
            Debug.Log("Defeat: Record not updated.");
        }
        
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameOverScene");
    }
}

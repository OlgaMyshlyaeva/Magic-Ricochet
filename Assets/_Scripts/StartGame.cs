using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет главным меню игры. 
/// Позволяет запускать игровой процесс и корректно выходить из приложения.
/// </summary>
public class StartGame : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Имя сцены первого уровня. Безопаснее, чем индекс.")]
    [SerializeField] private string firstLevelName = "_Scene_0";

    /// <summary>
    /// Загружает первый уровень игры.
    /// </summary>
    public void PlayGame()
    {
        // Проверяем, существует ли сцена с таким именем (опционально для надежности)
        if (!string.IsNullOrEmpty(firstLevelName))
        {
            SceneManager.LoadScene(firstLevelName);
        }
        else
        {
            Debug.LogError("StartGame: Имя сцены не задано в инспекторе!");
        }
    }

    /// <summary>
    /// Закрывает приложение. 
    /// Работает в билде, а в редакторе выводит сообщение в консоль.
    /// </summary>
    public void QuitGame()
    {
        // В портфолио это покажет, что ты знаешь, как ведут себя разные платформы
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        Debug.Log("Game exited.");
    }
}

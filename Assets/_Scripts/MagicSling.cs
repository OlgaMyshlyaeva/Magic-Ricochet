using UnityEngine;

/// <summary>
/// Handles projectile spawning and launching toward the cursor.
/// </summary>
public class MagicSling : MonoBehaviour
{
    [Header("Launcher Settings")]
    public GameObject spellPrefab;
    public Camera spellCamera;
    public float fireForce = 25f; 
    public float projectileLifeTime = 2.5f; // Ограничено для предотвращения случайных попаданий

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0)) 
        {
            LaunchProjectile();
        }
        */
    }

    public void LaunchProjectile()
    {
        if (spellPrefab == null) return;
        if (spellCamera == null) return;

        Vector3 screenPosition;

        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }
        else // Если касаний нет, используем позицию мыши
        {
            screenPosition = Input.mousePosition;
        }

        // Определение направления выстрела через Raycast из камеры
        Ray ray = spellCamera.ScreenPointToRay(screenPosition);

        // Создание снаряда с отступом от камеры
        GameObject ball = Instantiate(spellPrefab, spellCamera.transform.position + ray.direction * 1.2f, Quaternion.identity);

        if (ball.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // VelocityChange игнорирует массу, обеспечивая одинаковую скорость для всех снарядов
            rb.AddForce(ray.direction * fireForce, ForceMode.VelocityChange);
        }

        // Автоматическая очистка памяти
        Destroy(ball, projectileLifeTime);
    }
}


using UnityEngine;

/// <summary>
/// Handles projectile spawning and launching toward the cursor.
/// </summary>
public class MagicSling : MonoBehaviour
{
    [Header("Launcher Settings")]
    public GameObject spellPrefab;
    public float fireForce = 25f; 
    public float projectileLifeTime = 2.5f; // Ограничено для предотвращения случайных попаданий

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            LaunchProjectile();
        }
    }

    private void LaunchProjectile()
    {
        if (spellPrefab == null) return;

        // Определение направления выстрела через Raycast из камеры
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Создание снаряда с отступом от камеры
        GameObject ball = Instantiate(spellPrefab, transform.position + ray.direction * 1.2f, Quaternion.identity);
        
        if (ball.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // VelocityChange игнорирует массу, обеспечивая одинаковую скорость для всех снарядов
            rb.AddForce(ray.direction * fireForce, ForceMode.VelocityChange);
        }

        // Автоматическая очистка памяти
        Destroy(ball, projectileLifeTime);
    }
}


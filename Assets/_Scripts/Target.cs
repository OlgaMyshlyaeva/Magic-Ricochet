using UnityEngine;

/// <summary>
/// Handles breakable environmental objects.
/// </summary>
public class Target : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float breakForce = 5f;
    [SerializeField] private GameObject explosionPrefab;

    public bool IsDestroyed { get; private set; } = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsDestroyed) return;

        if (collision.relativeVelocity.magnitude > breakForce)
        {
            Die();
        }
    }

    public void Die()
    {
        if (IsDestroyed) return;
        IsDestroyed = true;

        if (explosionPrefab != null) 
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // Вызов метода теперь не вызовет ошибку
        if (GameManager.S != null) GameManager.S.OnEnemyDefeated();

        if (CameraShake.S != null) CameraShake.S.Shake(0.15f, 0.2f);

        // Скрываем объект, чтобы GameManager его не видел как "живой", но удаляем чуть позже
        gameObject.SetActive(false); 
        Destroy(gameObject, 0.1f); 
    }
}


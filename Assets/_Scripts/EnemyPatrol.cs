using UnityEngine;
using System.Collections;

/// <summary>
/// Controls enemy behavior including patrolling, health management, and death effects.
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
    public enum EnemyType { Normal, Red, Gold, Black }

    [Header("Enemy Configuration")]
    [Tooltip("Type determines health, visuals, and game rules on death")]
    public EnemyType type = EnemyType.Normal;
    public int health = 1;
    
    [Header("Movement Settings")]
    public float speed = 2f;
    public float distance = 3f;

    [Header("Visual Assets")]
    [SerializeField] private Material goldMaterial; 
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blackMaterial;
    
    private Vector3 _startPos;
    [HideInInspector] public bool isDead = false; // Флаг для GameManager
    private MeshRenderer _renderer;
    private Rigidbody _rb;
    public GameObject PlusFiveSecondPNG;
    public GameObject BoomPNG;

    void Awake()
    {
        // Кэширование компонентов для оптимизации
        _renderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _startPos = transform.position;
        InitializeEnemy();
    }

    /// <summary>
    /// Sets initial health and visuals based on the enemy type.
    /// </summary>
    private void InitializeEnemy()
    {
        // Настройка здоровья: Золотой — 3 HP, остальные — 1 HP
        health = (type == EnemyType.Gold) ? 3 : 1;

        if (_renderer == null) return;

        // Применение соответствующего материала
        switch (type)
        {
            case EnemyType.Gold: if (goldMaterial) _renderer.material = goldMaterial; break;
            case EnemyType.Red: if (redMaterial) _renderer.material = redMaterial; break;
            case EnemyType.Black: if (blackMaterial) _renderer.material = blackMaterial; break;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Синусоидальное патрулирование (независимо от физики)
        float newX = _startPos.x + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Уменьшаем здоровье и уничтожаем снаряд
            health--;
            Destroy(collision.gameObject); 

            if (health <= 0) 
            {
                Die();
            }
            else 
            {
                // Визуальный фидбек при попадании (не смертельном)
                if (CameraShake.S) CameraShake.S.Shake(0.1f, 0.15f);
                Debug.Log($"{gameObject.name} took damage! Remaining HP: {health}");
            }
        }
    }

    /// <summary>
    /// Handles death logic and notifies the Game Manager.
    /// </summary>
    private void Die()
    {
        

        if (isDead) return;
        isDead = true;

        if (type == EnemyType.Black)
        {
            // Попадание в черного скелета — мгновенный проигрыш
            GameManager.S.GameOver(false); 
        }
        else 
        {
            // Бонус времени за красного скелета
            if (type == EnemyType.Red) 
            { 
                GameManager.S.AddBonusTime(5f); 
                GameObject spawnedObject = Instantiate(PlusFiveSecondPNG, transform.position, Quaternion.identity);
                StartCoroutine(AnimateAndDestroy(spawnedObject)); 
            }
            // ПРИМЕЧАНИЕ: GameManager сам проверит отсутствие целей в Update
            if (type == EnemyType.Gold)
            {
                GameObject spawnedObject = Instantiate(BoomPNG, transform.position, Quaternion.identity);
                StartCoroutine(AnimateAndDestroy(spawnedObject));
            }

        }

        // Эффект физического "отлета" при смерти
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
            _rb.AddForce(Vector3.up * 8f + Random.insideUnitSphere * 2f, ForceMode.Impulse);
            _rb.AddTorque(Random.insideUnitSphere * 50f, ForceMode.Impulse);
        }

        if (CameraShake.S) CameraShake.S.Shake(0.2f, 0.3f);
        Destroy(gameObject, 1.5f);
    }


    //Код для появления картинки "+5Секунд" или "Бум!" и плавное поднятие его вверх по времени
    private IEnumerator AnimateAndDestroy(GameObject objToAnimate)
    {
        Vector3 startPosition = objToAnimate.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 5f;

        float elapsedTime = 0f;


        while (elapsedTime < 0.3f)
        {

            float t = elapsedTime;

            objToAnimate.transform.position = Vector3.Lerp(startPosition, endPosition, t);


            elapsedTime += Time.deltaTime;

            if (Time.timeScale == 0) { break; }

            yield return null;
        }


        yield return new WaitForSeconds(0.1f);

        Destroy(objToAnimate);


    }



}

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
    public GameObject Minus1HP;
    public GameObject Now1HPBoss;
    public Sprite BossBar3HP;
    public Sprite BossBar2HP;
    public Sprite BossBar1HP;

    private SoundsMaster soundsMasterScript;
    private GameObject TextTimer;
    private TextAnimations1 ScriptAnimTimer;


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
        GameObject soundsMaster = GameObject.Find("SoundMaster");
        soundsMasterScript = soundsMaster.GetComponent<SoundsMaster>();
        TextTimer = GameObject.Find("TimerText");
        if (TextTimer != null) { ScriptAnimTimer = TextTimer.GetComponent<TextAnimations1>(); }
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



        //Отображение бара хп (для босса)
        Transform BossBar = transform.Find("boss_hp_bar");
        if (BossBar != null && type == EnemyType.Gold)
        {
            BossBar.gameObject.SetActive(true);
            SpriteRenderer spriteRenderer = BossBar.GetComponent<SpriteRenderer>();
            if (health == 3) { spriteRenderer.sprite = BossBar3HP; }
            if (health == 2) { spriteRenderer.sprite = BossBar2HP; }
            if (health == 1) { spriteRenderer.sprite = BossBar1HP; }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Уменьшаем здоровье и уничтожаем снаряд
            health--;
            //Destroy(collision.gameObject); 

            if (health <= 0) 
            {
                //Отображение бара хп (для босса)
                Transform BossBar = transform.Find("boss_hp_bar");
                if (BossBar != null && type == EnemyType.Gold)
                {
                    SpriteRenderer spriteRenderer = BossBar.GetComponent<SpriteRenderer>();
                    BossBar.gameObject.SetActive(false); spriteRenderer.enabled = false;
                }

                Die();
                soundsMasterScript.PlaySoundScatteringofbones(); //Звук смерти скелета
            }
            else 
            {
                // Визуальный фидбек при попадании (не смертельном)
                if (CameraShake.S) CameraShake.S.Shake(0.1f, 0.15f);
                //Debug.Log($"{gameObject.name} took damage! Remaining HP: {health}");
                soundsMasterScript.PlaySoundBonecrunching();    //Звук костей при попадании в скелета с HP>1


                //Вылет popup`ов для босса
                if (type == EnemyType.Gold)
                {
                    if (health>=2) 
                    {
                        GameObject spawnedObject = Instantiate(Minus1HP, transform.position, Quaternion.identity);
                        StartCoroutine(AnimateAndDestroy(spawnedObject));
                    }
                    if (health==1)
                    {
                        GameObject spawnedObject = Instantiate(Now1HPBoss, transform.position, Quaternion.identity);
                        StartCoroutine(AnimateAndDestroy(spawnedObject));
                    }
                }

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
                soundsMasterScript.PlaySoundAsimplebell();  //Проигрывание звука колокольчика
                ScriptAnimTimer.startAnim = true;
            }

            if (type == EnemyType.Gold)
            {
                GameObject spawnedObject = Instantiate(BoomPNG, transform.position, Quaternion.identity);
                StartCoroutine(AnimateAndDestroy(spawnedObject));
            }

            // ПРИМЕЧАНИЕ: GameManager сам проверит отсутствие целей в Update
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




    public IEnumerator AnimateAndDestroy(GameObject spawnedObject)
    {
        
        Vector3 startPosition = spawnedObject.transform.position;

        Vector3 endPosition = startPosition + Vector3.up * 3f;

        float elapsedTime = 0f;

        // Цикл для анимации подъема
        while (elapsedTime < 0.5f)
        {
 
            float t = elapsedTime / 0.5f;

            spawnedObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(spawnedObject);
    }









}

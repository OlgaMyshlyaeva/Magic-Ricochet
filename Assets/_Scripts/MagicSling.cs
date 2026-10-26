using UnityEngine;

/// <summary>
/// Handles projectile spawning and launching toward the cursor.
/// </summary>
public class MagicSling : MonoBehaviour
{
    [Header("Launcher Settings")]
    public GameObject spellPrefab;
    public float fireForce = 25f; 
    private float projectileLifeTime = 30.5f; // Ограничено для предотвращения случайных попаданий
    private float delayBetweenCalls = 0.1f; // Пауза между выстрелами
    private float lastCallTime = -Mathf.Infinity; //Время последнего выстрела
    private SoundsMaster soundsMasterScript;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            LaunchProjectile();
        }
    }
    private void Start()
    {
        GameObject soundsMaster = GameObject.Find("SoundMaster");
        soundsMasterScript = soundsMaster.GetComponent<SoundsMaster>();
    }

    private void LaunchProjectile()
    {
        if (spellPrefab == null) return;
<<<<<<< Updated upstream

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
=======
        if (spellCamera == null) return;

        if ((Time.time - lastCallTime) >= delayBetweenCalls)
        {
            lastCallTime = Time.time;
            Vector3 screenPosition;


            int randomNumber = Random.Range(1, 5);
            switch (randomNumber)
            {
                //Звук выпускания шара
                case 1: soundsMasterScript.PlaySoundFireballrelease(); break;

                //Звук полета огненного шара 1
                case 2: soundsMasterScript.PlaySoundFireballFlight(); break;

                //Звук полета огненного шара 2
                case 3: soundsMasterScript.PlaySoundFireballflight2(); break;

                //Звук полета огненного шара 3
                case 4: soundsMasterScript.PlaySoundFlightofthefireball3(); break;
            }
            

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
>>>>>>> Stashed changes
    }
}


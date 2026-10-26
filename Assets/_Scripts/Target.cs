using UnityEngine;
using static EnemyPatrol;

/// <summary>
/// Handles breakable environmental objects.
/// </summary>
public class Target : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float breakForce = 5f;
    [SerializeField] private GameObject explosionPrefab;

    public bool IsDestroyed { get; private set; } = false;
    private SoundsMaster soundsMasterScript;

    private void OnCollisionEnter(Collision collision)
    {
        Target targetScript = collision.gameObject.GetComponent<Target>();

        if (IsDestroyed) return;

        if (targetScript == null || targetScript.IsDestroyed)
        {
            if (collision.relativeVelocity.magnitude > breakForce)
            {
                Die();

                //Проигрывание звука
                if (collision.gameObject.tag == "BackWallForDestroingBalls")
                {
                    int randomNumber = Random.Range(1, 5);
                    switch (randomNumber)
                        {
                        //Звук промаха шара 1
                        case 1: soundsMasterScript.PlaySoundBell_miss1(); break;

                        //Звук промаха шара 2
                        case 2: soundsMasterScript.PlaySoundBell_miss2(); break;

                        //Звук промаха шара 3
                        case 3: soundsMasterScript.PlaySoundBell_miss3(); break;

                        //Звук промаха шара 4
                        case 4: soundsMasterScript.PlaySoundBell_miss4(); break;
                        }
                }
                else
                {
                    if (CameraShake.S != null) CameraShake.S.Shake(0.15f, 0.2f);
                    if (collision.gameObject.tag == "Enemies")
                    {
                        int randomNumber = Random.Range(1, 4);
                        switch (randomNumber)
                        {
                            //Звук попадание шара 1
                            case 1: soundsMasterScript.PlaySoundHittingafireball(); break;

                            //Звук попадание шара 2
                            case 2: soundsMasterScript.PlaySoundFireballHit2(); break;

                            //Звук попадание шара 3
                            case 3: soundsMasterScript.PlaySoundFireballhit3(); break;
                        }
                    }
                    else
                    {
                        soundsMasterScript.PlaySoundFireballExplosion(); //Звук взрыва шара
                    }
                }

            }
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

        

        // Скрываем объект, чтобы GameManager его не видел как "живой", но удаляем чуть позже
        gameObject.SetActive(false);

        
        Destroy(gameObject, 0.1f); 
    }




    private void Start()
    {
        GameObject soundsMaster = GameObject.Find("SoundMaster");
        soundsMasterScript = soundsMaster.GetComponent<SoundsMaster>();
    }
}


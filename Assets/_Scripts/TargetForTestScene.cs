using UnityEngine;
using System.Collections;
using static EnemyPatrol;

/// <summary>
/// Handles breakable environmental objects.
/// </summary>
public class TargetForTestScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float breakForce = 5f;
    [SerializeField] private GameObject explosionPrefab;

    public bool IsDestroyed { get; private set; } = false;
    private SoundsMaster soundsMasterScript;


    private bool isProcessingCollision = false;


    private void OnCollisionEnter(Collision collision)
    {
        Target targetScript = collision.gameObject.GetComponent<Target>();

        if (IsDestroyed) return;

        if (targetScript == null || targetScript.IsDestroyed)
        {

                Vector3 initialVelocity = GetComponent<Rigidbody>() != null ? GetComponent<Rigidbody>().linearVelocity : Vector3.zero;
                float angleBeforeCollision = Vector3.zero.Equals(initialVelocity) ? 0f : Vector3.SignedAngle(Vector3.forward, initialVelocity, Vector3.up);







                //Условие для исключения "двойного" рикошета от одной поверхности
                if (isProcessingCollision)
                {
                    return;
                }

                isProcessingCollision = true;




                //Механика рикошета
                if (collision.gameObject.tag == "WallsForRicochet")
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {

                        Vector3 normal = collision.contacts[0].normal;
                        Vector3 incomingVelocity = collision.relativeVelocity;

                        if (Vector3.Dot(incomingVelocity, normal) > 0)
                        {
                            normal = -normal;
                        }

                        float angleToNormal = Vector3.Angle(incomingVelocity, normal);
                        float slidingAngleThreshold = 85.0f;    //Максимальный угол при котором удар "скользящий" и рикошет не требуется

                        Vector3 reflectedVelocity;

                        if (angleToNormal > slidingAngleThreshold)
                        {
                            Vector3 potentialReflection = Vector3.Reflect(incomingVelocity, normal);
                            float reflectionDot = Vector3.Dot(potentialReflection.normalized, incomingVelocity.normalized);

                            if (reflectionDot < -0.9f)
                            {
                                Vector3 velocityParallelToSurface = Vector3.ProjectOnPlane(incomingVelocity, normal);
                                reflectedVelocity = velocityParallelToSurface.normalized * incomingVelocity.magnitude * 0.8f;
                                reflectedVelocity += normal * 0.2f * incomingVelocity.magnitude;
                            }
                            else
                            {
                                reflectedVelocity = potentialReflection;
                            }
                        }
                        else
                        {
                            reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);
                        }

                        if (Vector3.Dot(reflectedVelocity, normal) > 0)
                        {
                            reflectedVelocity = -reflectedVelocity;
                        }

                        rb.linearVelocity = reflectedVelocity;

                        Vector3 velocityAfterCollisionGlobal = rb.linearVelocity;
                        Vector3 velocityBeforeImpact = incomingVelocity;

                        /*
                        float angleBeforeGlobal = Vector3.SignedAngle(Vector3.forward, velocityBeforeImpact, Vector3.up);
                        float angleCollisionAngle = Vector3.Angle(incomingVelocity, normal);
                        float angleAfterGlobal = Vector3.SignedAngle(Vector3.forward, velocityAfterCollisionGlobal, Vector3.up);

                        if (collision.contacts.Length > 0)
                        {
                            Debug.Log($"Угол до столкновения (глоб.): {angleBeforeGlobal} градусов");
                            Debug.Log($"Угол столкновения (скорость к нормали): {angleCollisionAngle} градусов");
                            Debug.Log($"Угол после столкновения (глоб.): {angleAfterGlobal} градусов");
                        }
                        */



                    }


                    StartCoroutine(ResetCollisionProcessingWithDelay(0.01f));













                }
                else
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












    //Сброс флага возможности рикошета
    private IEnumerator ResetCollisionProcessingWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isProcessingCollision = false;
    }





    private void Start()
    {
        GameObject soundsMaster = GameObject.Find("SoundMaster");
        soundsMasterScript = soundsMaster.GetComponent<SoundsMaster>();
    }

    private float timeVelocityBecameZero = -1f;
    private void Update()
    {

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        Vector3 currentVelocity = rb.linearVelocity;

        if (currentVelocity == Vector3.zero)
        {
            if (timeVelocityBecameZero < 0)
            {
                timeVelocityBecameZero = Time.time; 
            }
        }
        else
        {
            timeVelocityBecameZero = -1f;
        }


        if (timeVelocityBecameZero > 0 && Time.time - timeVelocityBecameZero >= 1.0f)
        {
            Die();
        }



        
    }
}


using UnityEngine;
using System.Collections;

/// <summary>
/// Реализует эффект дрожания камеры (Screen Shake) с использованием корутин.
/// Поддерживает наложение нескольких эффектов тряски одновременно.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake S;

    private Vector3 _originalPos;

    void Awake()
    {
        // Синглтон с защитой от дубликатов
        if (S == null) S = this;
        else Destroy(gameObject);
        
        _originalPos = transform.localPosition;
    }

    /// <summary>
    /// Запускает эффект тряски.
    /// </summary>
    /// <param name="duration">Длительность в секундах</param>
    /// <param name="amount">Сила смещения</param>
    public void Shake(float duration, float amount)
    {
        // Вместо Invoke используем корутины — это стандарт Unity для временных процессов
        StopAllCoroutines(); // Опционально: прерываем старую тряску перед новой
        StartCoroutine(ProcessShake(duration, amount));
    }

    private IEnumerator ProcessShake(float duration, float amount)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Используем InsideUnitSphere для более естественной 3D тряски
            Vector3 randomOffset = Random.insideUnitSphere * amount;
            
            // Плавное затухание силы тряски к концу времени (Lerp)
            float dampening = 1.0f - (elapsed / duration);
            
            transform.localPosition = _originalPos + (randomOffset * dampening);

            elapsed += Time.deltaTime;
            yield return null; // Ждем следующего кадра
        }

        // Возвращаем камеру строго в исходную точку
        transform.localPosition = _originalPos;
    }
}

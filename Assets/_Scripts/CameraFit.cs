using UnityEngine;

/// <summary>
/// Adjusts the Orthographic Camera size to fit a specific scene width.
/// Ensures consistent gameplay view across different screen aspect ratios.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    [Header("Resolution Settings")]
    [Tooltip("The desired width of the level in Unity units.")]
    public float sceneWidth = 20f;

    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        AdjustCamera();
    }

    /// <summary>
    /// Calculates and sets the camera's orthographic size based on screen aspect ratio.
    /// </summary>
    public void AdjustCamera()
    {
        if (_cam == null) return;

        // Formula: Size = (Width / Aspect Ratio) / 2
        float aspectRatio = (float)Screen.width / Screen.height;
        float desiredHalfHeight = (sceneWidth / aspectRatio) * 0.5f;

        _cam.orthographicSize = desiredHalfHeight;
    }

    // Live update in the Editor for easier testing
#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying) 
        {
            AdjustCamera();
        }
    }

    void OnValidate()
    {
        if (_cam == null) _cam = GetComponent<Camera>();
        AdjustCamera();
    }
#endif
}


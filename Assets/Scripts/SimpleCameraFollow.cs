using UnityEngine;

/// <summary>
/// Следование камеры за игроком в 2D: только X/Y, Z камеры фиксирован (обычно -10
/// для ортографической камеры, чтобы всё было перед ней).
/// </summary>
public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = Vector2.zero;
    public float smoothTime = 0.2f;
    public float fixedZ = -10f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(target.position.x + offset.x, target.position.y + offset.y, fixedZ);
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}

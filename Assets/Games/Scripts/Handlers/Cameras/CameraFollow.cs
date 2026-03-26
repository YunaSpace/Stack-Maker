using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    public float Speed = 5f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        Vector3 targetPos = Target.position + Offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.1f);
    }
}
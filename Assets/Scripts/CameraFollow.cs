using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float followSpeed = 10f;

    [SerializeField]
    private float yOffset = 1.5f;

    [SerializeField]
    private Transform target;

    private void FixedUpdate()
    {
        var newPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed * Time.deltaTime);
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Reference to the player's transform
    public Vector3 offset;        // Offset between the player and the camera
    public float smoothSpeed = 0.125f;  // Camera smoothness factor

    void LateUpdate()
    {
        // Calculate the desired camera position with an offset
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position
        transform.position = smoothedPosition;
    }
}

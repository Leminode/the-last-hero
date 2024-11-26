using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MovableObject : MonoBehaviour
{
    [SerializeField]
    private AudioClip movingSound; // The sound to play when the object is moving

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isMoving;

    private void Awake()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.clip = movingSound;
        audioSource.loop = true; // Loop the sound for continuous playback
        audioSource.playOnAwake = false; // Do not play automatically
    }

    private void FixedUpdate()
    {
        // Check if the object is moving
        if (rb.velocity.magnitude > 0.1f)
        {
            if (!isMoving)
            {
                StartMoving();
            }
        }
        else
        {
            if (isMoving)
            {
                StopMoving();
            }
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        audioSource.Play();
    }

    private void StopMoving()
    {
        isMoving = false;
        audioSource.Stop();
    }
}

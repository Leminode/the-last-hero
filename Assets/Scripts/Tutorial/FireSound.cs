using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Fixed2DSound : MonoBehaviour
{
    [SerializeField]
    private Transform player; // Reference to the player's Transform

    [SerializeField]
    private float maxVolume = 1f; // Maximum volume when the player is close
    [SerializeField]
    private float minVolume = 0f; // Minimum volume when the player is far
    [SerializeField]
    private float maxDistance = 5f; // Maximum distance to hear the sound

    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from " + gameObject.name);
            return;
        }

        // Ensure the AudioSource is looping and starts playing
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set in " + gameObject.name);
            return;
        }

        // Calculate distance between the player and this sound source
        float distance = Vector2.Distance(transform.position, player.position);

        // Adjust the volume based on the distance
        if (distance <= maxDistance)
        {
            audioSource.volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);
        }
        else
        {
            audioSource.volume = minVolume; // Volume is 0 when outside max distance
        }
    }
}

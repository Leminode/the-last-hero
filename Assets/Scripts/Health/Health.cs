using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;

    [SerializeField]
    private bool restartLevel;

    [SerializeField]
    private float respawnTime = 1f;

    [SerializeField]
    private AudioClip deathSound; // Death sound clip

    [SerializeField]
    private AudioClip hitSound; // Hit sound clip

    private Animator _animator;
    private Rigidbody2D _rb;
    private AudioSource _audioSource; // To play sounds

    public bool IsDead { get; private set; }
    public float CurrentHealth { get; private set; }

    protected virtual void Start()
    {
        CurrentHealth = startingHealth;
        _animator = GetComponent<Animator>();

        if (restartLevel)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Get or add an AudioSource to this GameObject
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for consistent playback
        _audioSource.playOnAwake = false;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);

        if (CurrentHealth > 0)
        {
            _animator.SetTrigger("hurt");

            // Play the hit sound when damaged
            PlayHitSound();
        }
        else if (!IsDead)
        {
            IsDead = true;
            _animator.SetTrigger("die");

            // Play death sound and handle post-death actions
            PlayDeathSound();

            if (restartLevel)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(RestartLevel());
            }
            else
            {
                StartCoroutine(DestroyAfterSound());
            }
        }
    }

    private void PlayHitSound()
    {
        if (hitSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(hitSound); // Play hit sound effect
        }
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(deathSound); // Play death sound effect
        }
        else
        {
            Debug.LogWarning("Death sound is missing or AudioSource is not configured properly.");
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(respawnTime);
        GameManager.Instance.RestartLevel();
    }

    private IEnumerator DestroyAfterSound()
    {
        if (deathSound != null && _audioSource != null)
        {
            // Wait for the length of the death sound before destroying the GameObject
            yield return new WaitForSeconds(deathSound.length);
        }
        Destroy(gameObject);
    }
}

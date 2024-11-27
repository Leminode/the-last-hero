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
    private AudioClip damageSound; // Add damage sound clip here

    [SerializeField]
    private AudioClip deathSound; // Optional: Death sound (if needed)

    private Animator _animator;
    private Rigidbody2D _rb;
    private AudioSource _audioSource;

    public bool IsDead { get; private set; }
    public float CurrentHealth { get; private set; }

    protected virtual void Start()
    {
        CurrentHealth = startingHealth;
        _animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (restartLevel)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }


    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);

        // Play damage sound
        if (damageSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(damageSound);
        }

        if (CurrentHealth > 0)
        {
            _animator.SetTrigger("hurt");
        }
        else if (!IsDead)
        {
            IsDead = true;
            _animator.SetTrigger("die");

            // Optionally play death sound
            if (deathSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(deathSound);
            }

            if (restartLevel)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(RestartLevel());
            }
            else
            {
                Destroy(gameObject, deathSound != null ? deathSound.length : 0f); // Delay destruction
            }

        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(respawnTime);
        GameManager.Instance.RestartLevel();
    }
}

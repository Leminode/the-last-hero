using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Died;

    [SerializeField]
    private float startingHealth;

    [SerializeField]
    private bool restartLevel;
    
    [SerializeField]
    private float respawnTime = 1f;

    private Animator _animator;

    private Rigidbody2D _rb;

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
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);

        if (CurrentHealth > 0)
        {
            _animator.SetTrigger("hurt");
        }
        else if (!IsDead)
        {
            IsDead = true;
            _animator.SetTrigger("die");

            if (restartLevel)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(RestartLevel());
            }
            else
            {
                gameObject.SetActive(false);
            }

            Died.Invoke();
        }
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(respawnTime);
        GameManager.Instance.RestartLevel();
    }
}

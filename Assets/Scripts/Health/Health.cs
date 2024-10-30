using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;

    public float currentHealth { get; private set; }

    private Animator animator;

    private bool isDead;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
        }
        else if (!isDead)
        {
            animator.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
            isDead = true;
        }
    }
}

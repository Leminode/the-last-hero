using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;

    public float currentHealth { get; private set; }

    private Animator animator;
    private Rigidbody2D rb;

    private bool isDead;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
            isDead = true;
            animator.SetTrigger("die");
            // TODO: only for demo purposes
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            StartCoroutine(Respawn());
        }
    }
    
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(3.61f, -4.58217f, 0);
        currentHealth = startingHealth;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDead = false;
    }
}

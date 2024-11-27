using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;

    private Animator anim;
    private UIManager uiManager;

    public bool IsDead { get; private set; }
    public float CurrentHealth { get; private set; }

    protected virtual void Start()
    {
        CurrentHealth = startingHealth;
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);

        if (CurrentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else if (!IsDead)
        {
            IsDead = true;
            anim.SetTrigger("die");

            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (CompareTag("Player"))
            {
                uiManager.GameOver();
            }
            else
            {
                Destroy(gameObject, 1f);
            }
        }
    }

    // Triggered on animation event (if set)
    public void DisableAnimator()
    {
        print("Disabling animator");
        GetComponent<Animator>().enabled = false;
    }
}

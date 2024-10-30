using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown;

    private Animator animator;
    private PlayerMovement playerMovement;

    //Audio source
    public AudioSource hitSFX;

    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement.CanAttack())
        {
            Attack();
            hitSFX.Play();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        cooldownTimer = 0;
    }
}

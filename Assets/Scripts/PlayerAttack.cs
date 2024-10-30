using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float range;

    [SerializeField]
    private float damage;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private float colliderDistance;

    [SerializeField]
    private LayerMask enemyLayer;

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

    private Collider2D GetEnemyInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            enemyLayer);

        return hit.collider;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * transform.right,
            new(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DealDamageAtTheEndOfAttack()
    {
        var enemyInRange = GetEnemyInRange();
        if (enemyInRange == null)
            return;

        var enemyHealth = enemyInRange.GetComponent<Health>();
        if (enemyHealth == null)
            return;

        enemyHealth.TakeDamage(damage);
    }
}

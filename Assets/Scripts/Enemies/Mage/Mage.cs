using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private int damage;

    [Header("Player detection parameters")]
    [SerializeField]
    private float range;

    [SerializeField]
    private float colliderDistance;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask playerLayer;

    [Header("Fireballs")]
    [SerializeField]
    private Transform firepoint;

    [SerializeField]
    private GameObject[] fireballs;

    private Animator animator;

    private float mageAttackTimer = Mathf.Infinity;

    private const string mageAttackTriggerName = "mageAttack";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        mageAttackTimer += Time.deltaTime;
        if (mageAttackTimer >= attackCooldown && IsPlayerInSight())
        {
            mageAttackTimer = 0;
            MageAttack();
        }
    }

    private bool IsPlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * -transform.right,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0,
            Vector2.left,
            0,
            playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + colliderDistance * range * transform.localScale.x * -transform.right,
            new(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void MageAttack()
    {
        mageAttackTimer = 0;
        var activeFireBall = FindFireball();
        fireballs[activeFireBall].transform.position = firepoint.position;
        fireballs[activeFireBall].GetComponent<MageProjectile>().SetDirection(Mathf.Sign(-transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i< fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }

        return 0;
    }
}

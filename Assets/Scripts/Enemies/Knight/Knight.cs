using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float colliderDistance;

    [SerializeField]
    private float damageCooldown;

    [SerializeField]
    private float directionChangeCooldown;

    [SerializeField]
    private float playerDetectionRange;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private BoxCollider2D boxCollider;

    private int direction;
    private Transform player;
    private float directionChangeCooldownTimer = Mathf.Infinity;
    private Rigidbody2D rigibody;
    private Vector3 initialScale;

    private System.Random random = new();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigibody = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
    }

    private void Update()
    {
        directionChangeCooldownTimer += Time.deltaTime;

        if (directionChangeCooldownTimer >= directionChangeCooldown)
        {
            directionChangeCooldownTimer = 0;
            direction = random.Next(-1, 1) == -1 ? -1 : 1;
        }

        if (IsPlayerDetected())
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rigibody.position = new Vector2(
                rigibody.position.x + Time.deltaTime * speed * directionToPlayer.x,
                rigibody.position.y + Time.deltaTime * speed * directionToPlayer.y
                );
        }
        else
        {
            rigibody.position = new Vector2(
                rigibody.position.x + Time.deltaTime * speed * direction,
                rigibody.position.y + Time.deltaTime * speed * direction
                );

            transform.localScale = new Vector2(
                direction,
                initialScale.y
                );
        }
    }

    private bool IsPlayerDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + colliderDistance * playerDetectionRange * transform.localScale.x * transform.right,
            new Vector3(boxCollider.bounds.size.x * playerDetectionRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
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
            boxCollider.bounds.center + colliderDistance * playerDetectionRange * transform.localScale.x * transform.right,
            new(boxCollider.bounds.size.x * playerDetectionRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}

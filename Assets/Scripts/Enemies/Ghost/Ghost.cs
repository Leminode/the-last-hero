using UnityEngine;
using UnityEngine.EventSystems;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float damageCooldown;

    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    private float directionChangeCooldown;

    [SerializeField]
    private float areaRadius;

    [SerializeField]
    private Vector2 areaCenter;

    private Vector2 moveDirection;

    private Vector2 direction;
    private float directionChangeCooldownTimer = Mathf.Infinity;
    private float damageCooldownTimer = Mathf.Infinity;

    private Transform player;
    private Rigidbody2D rigibody;

    private readonly System.Random random = new();
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigibody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        directionChangeCooldownTimer += Time.deltaTime;
        damageCooldownTimer += Time.deltaTime;

        if (directionChangeCooldownTimer >= directionChangeCooldown)
        {
            directionChangeCooldownTimer = 0;
            direction = RandomizeDirection();
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
                  rigibody.position.x + Time.deltaTime * speed * direction.x,
                  rigibody.position.y + Time.deltaTime * speed * direction.y
                  );

            ChangeDirectionIfOutsideArea();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageCooldownTimer >= damageCooldown)
        {
            player.GetComponent<Health>().TakeDamage(damage);
            damageCooldownTimer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageCooldownTimer >= damageCooldown)
        {
            player.GetComponent<Health>().TakeDamage(damage);
            damageCooldownTimer = 0;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(areaCenter, areaRadius);
    }

    private Vector2 RandomizeDirection()
    {
        float angle = Random.Range(0f, 360f);
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    private void ChangeDirectionIfOutsideArea()
    {
        Vector2 offset = (Vector2)transform.position - areaCenter;
        if (offset.magnitude > areaRadius)
        {
            direction = Vector2.Reflect(direction, offset.normalized);
            transform.position = areaCenter + offset.normalized * areaRadius;
        }
    }

    private bool IsPlayerDetected()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRadius;
    }
}

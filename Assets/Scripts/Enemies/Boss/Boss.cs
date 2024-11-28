using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float directionChangeCooldown;

    [SerializeField]
    private float areaRadius;

    [SerializeField]
    private Vector2 areaCenter;

    [SerializeField]
    private Transform firepoint1;

    [SerializeField]
    private GameObject[] fireballs1;

    [SerializeField]
    private Transform firepoint2;

    [SerializeField]
    private GameObject[] fireballs2;

    private Vector2 direction;
    private float directionChangeCooldownTimer = Mathf.Infinity;
    private float attackTimer = Mathf.Infinity;

    private bool eventTriggered = false;

    private Transform player;
    private Rigidbody2D rigibody;
    private UnityEvent gameWonEvent;
    private Health health;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigibody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        gameWonEvent = new UnityEvent();
    }

    private void Start()
    {
        gameWonEvent.AddListener(GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().WinScreen);
    }

    private void Update()
    {
        if (health.IsDead)
        {
            if (!eventTriggered)
            {
                gameWonEvent.Invoke();
                eventTriggered = true;
            }

            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown && IsPlayerDetected())
        {
            attackTimer = 0;
            Attack();
        }

        directionChangeCooldownTimer += Time.deltaTime;

        if (directionChangeCooldownTimer >= directionChangeCooldown)
        {
            directionChangeCooldownTimer = 0;
            direction = RandomizeDirection();
        }

        rigibody.position = new Vector2(
              rigibody.position.x + Time.deltaTime * speed * direction.x,
              rigibody.position.y + Time.deltaTime * speed * direction.y
              );

        ChangeDirectionIfOutsideArea();
    }
    public void OnDrawGizmos()
    {
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

    private void Attack()
    {
        var activeFirstFireBall = FindFirstFireball();
        fireballs1[activeFirstFireBall].transform.position = firepoint1.position;
        fireballs1[activeFirstFireBall].GetComponent<BossProjectile>().Enable();

        var activeSecondFireBall = FindSecondFireball();
        fireballs2[activeSecondFireBall].transform.position = firepoint2.position;
        fireballs2[activeSecondFireBall].GetComponent<BossProjectile>().Enable();
    }

    private int FindFirstFireball()
    {
        for (int i = 0; i < fireballs1.Length; i++)
        {
            if (!fireballs1[i].activeInHierarchy)
                return i;
        }

        return 0;
    }

    private int FindSecondFireball()
    {
        for (int i = 0; i < fireballs2.Length; i++)
        {
            if (!fireballs2[i].activeInHierarchy)
                return i;
        }

        return 0;
    }

    private bool IsPlayerDetected()
    {
        if (player == null)
            return false;

        float distanceToPlayer = Vector2.Distance(areaCenter, player.position);
        return distanceToPlayer <= areaRadius;
    }
}

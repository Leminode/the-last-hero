using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;                 // Patrol speed

    [SerializeField]
    private float detectionRange = 5f;            // Distance at which the skeleton detects the player

    [SerializeField]
    private float attackRange = 1.5f;             // Distance at which the skeleton starts attacking

    [SerializeField]
    private float deAggroRange = 10f;             // Distance at which the skeleton stops chasing

    [SerializeField]
    private float timeBetweenAttacks = 2f;        // Time between attacks (attack cooldown)

    [SerializeField]
    private float damage;

    private float lastAttackTime = 0f;            // Tracks the last time the skeleton attacked

    private Transform player;                     // Reference to the player's transform
    private Rigidbody2D body;                     // Skeleton's Rigidbody2D

    private int currentWaypointIndex = 0;         // Current waypoint index for patrolling
    private bool isChasingPlayer = false;         // Whether the skeleton is currently chasing the player

    [SerializeField]
    private Transform[] waypoints;                // Array of waypoints for patrolling

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate the distance between the skeleton and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Start chasing if player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            isChasingPlayer = true;
        }
        // Stop chasing and return to patrolling if player moves out of de-aggro range
        else if (distanceToPlayer >= deAggroRange)
        {
            isChasingPlayer = false;
        }

        if (isChasingPlayer)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();  // Patrol between waypoints when not chasing the player
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;  // If there are no waypoints, stop patrolling

        // Get the current waypoint position (we only care about X axis for horizontal movement)
        Vector2 targetPosition = new(waypoints[currentWaypointIndex].position.x, transform.position.y);

        // Move towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Flip the skeleton's sprite depending on the direction it's moving
        if (targetPosition.x > transform.position.x)
        {
            // Moving right
            transform.localScale = new Vector3(1, 1, 1);  // Default scale
        }
        else if (targetPosition.x < transform.position.x)
        {
            // Moving left
            transform.localScale = new Vector3(-1, 1, 1);  // Flip horizontally
        }

        // Check if skeleton reached the current waypoint
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;  // Loop through waypoints
        }
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange)
        {
            // Check if the skeleton can attack based on the time since the last attack
            if (Time.time >= lastAttackTime + timeBetweenAttacks)
            {
                // Stop moving when within attack range
                body.velocity = Vector2.zero;  // Stop the skeleton from moving further
                AttackPlayer();  // Start attacking the player
                lastAttackTime = Time.time;  // Update the time of the last attack
            }
        }
        else
        {
            // Move towards the player horizontally only, retain the current Y position
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Flip the skeleton's sprite depending on the direction it's moving (towards the player)
            if (player.position.x > transform.position.x)
            {
                // Player is to the right, face right
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (player.position.x < transform.position.x)
            {
                // Player is to the left, face left (flip the sprite)
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void AttackPlayer()
    {
        // Add attack behavior (e.g., damage the player or play an attack animation)
        Debug.Log("Skeleton is attacking the player!");
        // You can add logic here to deal damage to the player
        player.GetComponent<Health>().TakeDamage(damage);
    }
}

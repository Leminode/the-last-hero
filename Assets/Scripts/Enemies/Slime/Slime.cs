using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField]
    private float jumpCooldown;

    [SerializeField]
    private float directionChangeCooldonw;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    private float jumpCooldownTimer = Mathf.Infinity;
    private float directionChangeCooldownTimer = Mathf.Infinity;

    private int direction = 0; // either -1, 0, 1
    private bool isGrounded;

    private const string jumpingTriggerName = "Jumping";

    private Animator animator;
    private Rigidbody2D rigibody;

    private readonly System.Random random = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigibody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        jumpCooldownTimer += Time.deltaTime;
        directionChangeCooldownTimer += Time.deltaTime;

        if (jumpCooldownTimer >= jumpCooldown)
        {
            jumpCooldownTimer = 0;
            animator.SetBool(jumpingTriggerName, true);
            Jump();

            if (directionChangeCooldownTimer >= directionChangeCooldonw)
            {
                directionChangeCooldownTimer = 0;
                direction = random.Next(-1, 2);
            }
        }
        if (!isGrounded)
        {
            rigibody.position = new Vector2(
                rigibody.position.x + Time.deltaTime * speed * direction,
                rigibody.position.y);
        }
    }

    private void Jump()
    {
        rigibody.velocity = Vector2.up * jumpForce;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool(jumpingTriggerName, false);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // playerHealth.TakeDamage(damage);
        }
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask wallLayer;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float attackDamage;

    [SerializeField]
    private float attackBoxDistance;

    [SerializeField]
    private float decelerationDuration = 0.6f;

    [SerializeField]
    private AudioClip hitSound;

    [SerializeField]
    private AudioClip jumpSound;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;

    private float wallSlideSpeed;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private readonly float wallJumpingTime = 0.2f;
    private readonly float wallJumpingDuration = 0.3f;
    private Vector2 wallJumpingPower = new(5, 20f);

    private float attackCooldownTimer = Mathf.Infinity;
    private float decelerationTimer;
    private float attackHorizontalVelocity;

    private float horizontalInput;

    private bool isAttacking;
    private bool isHeavyAttack;
    private bool isDecelerating;
    private bool isJumping;
    private bool isWallJumping;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        wallSlideSpeed = speed / 6;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        isGrounded = CheckIsGrounded();
        isTouchingWall = CheckIsTouchingWall();
        isAttacking = Input.GetMouseButtonDown(0) && attackCooldownTimer > attackCooldown && isGrounded && !isTouchingWall;

        // Used to prevent grounded animation from playing when jumping
        if (isJumping && !isGrounded)
        {
            isJumping = false;
        }

        // Combat
        Attack();

        // Vertical movement
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;

            PlayerSoundManager.instance.PlaySound(jumpSound);
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping && !isAttacking)
        {
            Flip();

            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        // Stop moving after attacking
        if (isDecelerating)
        {
            decelerationTimer -= Time.deltaTime;
            float intensity = decelerationTimer / decelerationDuration;

            rb.velocity = new Vector2(Mathf.Lerp(0, attackHorizontalVelocity, intensity), rb.velocity.y);

            if (decelerationTimer <= 0)
            {
                isDecelerating = false;
            }
        }

        // Animations
        anim.SetBool("run", horizontalInput != 0 && rb.velocity.x != 0 && isGrounded && !isTouchingWall);
        anim.SetBool("grounded", isGrounded && !isJumping);
        anim.SetBool("wallSliding", isWallSliding);
        anim.SetBool("jumping", isJumping);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Attack()
    {
        if (isAttacking)
        {
            attackCooldownTimer = 0;

            PlayerSoundManager.instance.PlaySound(hitSound);
            anim.SetTrigger("attack");

            Invoke(nameof(DealDamage), 0.2f);

            if (Mathf.Abs(rb.velocity.x) >= 0.5f)
            {
                isHeavyAttack = true;
                isDecelerating = true;

                decelerationTimer = decelerationDuration;
                attackHorizontalVelocity = rb.velocity.x;
            }
        }
        else
        {
            attackCooldownTimer += Time.deltaTime;
        }
    }

    private void DealDamage()
    {
        var enemyInRange = GetEnemyInRange();
        if (enemyInRange == null || !enemyInRange.TryGetComponent<Health>(out var enemyHealth))
        {
            return;
        }

        float damage = isHeavyAttack ? attackDamage * 1.5f : attackDamage;
        enemyHealth.TakeDamage(damage);
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isJumping = true;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                transform.localScale = new Vector3(wallJumpingDirection, 1, 1);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void WallSlide()
    {
        if (isTouchingWall && !isGrounded && horizontalInput != 0 && !isWallJumping)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void Flip()
    {
        // Flip playermodel when changing movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private bool CheckIsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool CheckIsTouchingWall()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(col.bounds.center, new Vector2(transform.localScale.x, 0), col.bounds.extents.x + 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private Collider2D GetEnemyInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center + attackBoxDistance * attackRange * transform.localScale.x * transform.right,
            new Vector3(col.bounds.size.x * attackRange, col.bounds.size.y, col.bounds.size.z),
            0,
            Vector2.left,
            0,
            enemyLayer);

        return hit.collider;
    }

    private void OnDrawGizmos()
    {
        if (col == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            col.bounds.center + attackBoxDistance * attackRange * transform.localScale.x * transform.right,
            new(col.bounds.size.x * attackRange, col.bounds.size.y, col.bounds.size.z));
    }
}

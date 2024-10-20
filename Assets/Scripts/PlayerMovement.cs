using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private float wallJumpCooldown;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask wallLayer;

    private Rigidbody2D body;
    private BoxCollider2D colider;
    private Animator animator;

    private float defaultGravityScale;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        colider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        defaultGravityScale = body.gravityScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Flip playermodel when changing movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Animations
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", IsGrounded());
        animator.SetBool("falling", body.velocity.y < 0);

        if (wallJumpCooldown > 0.25f)
        {
            // Horizontal movement
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Stick to walls
            if (IsTouchingWall() && !IsGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = defaultGravityScale;
            }

            // Vertical movement
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }
    private void Jump()
    {
        if (IsGrounded())
        {
            // Jump
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
        else if (IsTouchingWall() && !IsGrounded())
        {
            if (horizontalInput == 0)
            {
                // De-attach from the wall
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 8, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Climb up the wall
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 10);
            }

            wallJumpCooldown = 0;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(colider.bounds.center, colider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsTouchingWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(colider.bounds.center, colider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !IsTouchingWall();
    }
}

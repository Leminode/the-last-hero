using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpPower;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask wallLayer;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;

    private float wallSlideSpeed;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private readonly float wallJumpingTime = 0.2f;
    private readonly float wallJumpingDuration = 0.3f;
    private Vector2 wallJumpingPower = new(8, 16f);


    //Audio sources
    public AudioSource jumpSFX;

    private float horizontalInput;

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

        if (isTouchingWall)
        {
            print("touching wall");
        }

        // Used to prevent grounded animation from playing when jumping
        if (isJumping && !isGrounded)
        {
            isJumping = false;
        }
        
        // Vertical movement
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;

            jumpSFX.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // TODO: fix upto fall, fall transition is too quick
        // TODO: wall jump does not unstick from wall, below condition for some reason always returns true
        // wall jumping gets turned off by wall sliding
        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }

        // Animations
        anim.SetBool("run", (horizontalInput != 0 || rb.velocity.x != 0) && isGrounded && !isTouchingWall);
        anim.SetBool("grounded", isGrounded && !isJumping);
        anim.SetBool("wallSliding", isWallSliding);
        anim.SetBool("jumping", isJumping);
        anim.SetFloat("yVelocity", rb.velocity.y);
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.01f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool CanAttack()
    {
        return horizontalInput == 0 && isGrounded && !isTouchingWall;
    }
}

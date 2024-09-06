using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    protected float horizontal;
    [SerializeField] protected float speed = 8f;
    [SerializeField] protected float jumpingPower = 16f;
    [SerializeField] protected float movementThreshold = 0.1f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float gravityScale = 3f;
    [SerializeField] protected float fallMultiplier = 2.5f;
    [SerializeField] protected float groundCheckDistance = 0.1f;
    protected bool isGrounded;

    [SerializeField] protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    protected Vector2 moveDirection;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb.gravityScale = gravityScale;
    }

    protected virtual void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        moveDirection = transform.right * horizontal;

        isGrounded = IsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(moveDirection.x * speed, rb.velocity.y);
        }
        else
        {
            rb.AddForce(moveDirection * speed * 0.2f, ForceMode2D.Force);
            float clampedXVelocity = Mathf.Clamp(rb.velocity.x, -speed, speed);
            rb.velocity = new Vector2(clampedXVelocity, rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    protected virtual bool IsGrounded()
    {
        int rayCount = 5; // Number of rays to cast
        float raySpacing = boxCollider.bounds.size.x / (rayCount - 1);
        Vector2 rayStart = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y);
        bool grounded = false;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = rayStart + Vector2.right * (raySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
            
            if (hit.collider != null)
            {
                grounded = true;
            }
            
            Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, hit.collider != null ? Color.green : Color.red);
        }

        return grounded;
    }

    protected virtual void Jump()
    {
        float jumpHorizontalVelocity = moveDirection.x != 0 ? Mathf.Sign(moveDirection.x) * speed : rb.velocity.x;
        Vector2 jumpVelocity = new Vector2(jumpHorizontalVelocity, jumpingPower);
        rb.velocity = jumpVelocity;
    }
}
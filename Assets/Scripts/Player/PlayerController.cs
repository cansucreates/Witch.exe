using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // movement settings
    public float moveSpeed = 8f;
    public float acceleration = 50f;
    public float deceleration = 50f;

    // component references
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    // input variables
    private float horizontalInput;
    private Vector2 velocity;

    // ground detection
    public LayerMask groundLayerMask = 1; // default to layer 1 (Ground)
    public float groundCheckDistance = 0.1f;
    public Transform groundCheckPoint;
    private bool isGrounded;

    // jump settings
    public float jumpForce = 15f;
    public float jumpCutMultiplier = 0.5f;
    private bool jumpPressed;
    private bool jumpHeld;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        GetInput();
        CheckGrounded();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        jumpHeld = Input.GetKey(KeyCode.Space);
    }

    void HandleMovement()
    {
        velocity = rb.linearVelocity;

        // calculate target speed
        float targetSpeed = horizontalInput * moveSpeed;

        if (horizontalInput != 0)
        {
            velocity.x = Mathf.MoveTowards(
                velocity.x,
                targetSpeed,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        // apply the calculated velocity
        rb.linearVelocity = velocity;

        // flip the player based on movement direction
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheckPoint.position,
            Vector2.down,
            groundCheckDistance,
            groundLayerMask
        );
        isGrounded = hit.collider != null; // check if the raycast hit something on the ground layer

        // Optionally, visualize the ground check in the editor
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(groundCheckPoint.position, Vector2.down * groundCheckDistance, rayColor);
    }

    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
        }

        if (!jumpHeld && velocity.y > 0)
        {
            velocity.y *= jumpCutMultiplier; // apply jump cut if jump is released early
            rb.linearVelocity = velocity;
        }
    }
}

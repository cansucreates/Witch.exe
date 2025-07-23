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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
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
}

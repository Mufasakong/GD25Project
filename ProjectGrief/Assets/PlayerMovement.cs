using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool platformerMode = false;

    [SerializeField] private Transform groundCheck; // Reference to the ground check transform
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius of the check
    [SerializeField] private LayerMask groundLayer; // Ground layer mask for filtering

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private bool jumpRequested;

    private bool isGrounded; // To store whether the player is grounded or not

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Perform the ground check to see if the player is on the ground using a Raycast
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);

        // If in platformer mode, check for movement and jump input
        if (platformerMode)
        {
            movementInput.y = 0; // Ensure no vertical movement is input when in platformer mode

            if (jumpRequested && isGrounded) // Check if we're grounded before allowing the jump
            {
                //ResetInputs();
                 // Reset jump
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpRequested = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        if (platformerMode)
        {
            if (context.performed && context.ReadValue<Vector2>().y > 0.5f)
            {
                if (isGrounded)
                {
                    jumpRequested = true;
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if (platformerMode)
        {
            // Smooth move
            float targetX = movementInput.x * moveSpeed;
            float smoothedX = Mathf.Lerp(rb.linearVelocity.x, targetX, 0.1f); // 0.1f = smoothing amount

            rb.linearVelocity = new Vector2(smoothedX, rb.linearVelocity.y);

            if (!isGrounded)
            {
                rb.gravityScale = 5f;
            }
        }
        else
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            rb.gravityScale = 0;
        }
    }


    public void ResetInputs()
    {
        Debug.Log("Resetting Inputs");
        movementInput = Vector2.zero;
        jumpRequested = false;
        rb.linearVelocity = Vector2.zero;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool platformerMode = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private bool jumpRequested;

    private bool isGrounded;
    
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);

        Vector2 animInput = movementInput.normalized;

        if (animator != null)
        {
            animator.SetFloat("MoveX", animInput.x);
            animator.SetFloat("MoveY", animInput.y);
            animator.SetBool("IsMoving", animInput.sqrMagnitude > 0.01f);
        }

        if (platformerMode)
        {
            movementInput.y = 0;

            if (jumpRequested && isGrounded)
            {
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
            float smoothedX = Mathf.Lerp(rb.linearVelocity.x, targetX, 0.1f);

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
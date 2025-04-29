using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 3f;

    public enum MovementMode { Free, HorizontalOnly, VerticalOnly }
    [SerializeField] private MovementMode movementMode = MovementMode.Free;

    [SerializeField] private bool shouldChase = true;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target == null || rb == null)
            return;

        if (!shouldChase)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        ApplyMovementConstraints();
        ChaseTarget();
        FlipSprite();
    }

    private void ChaseTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        switch (movementMode)
        {
            case MovementMode.Free:
                rb.linearVelocity = direction * moveSpeed;
                break;

            case MovementMode.HorizontalOnly:
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, 0f);
                break;

            case MovementMode.VerticalOnly:
                rb.linearVelocity = new Vector2(0f, direction.y * moveSpeed);
                break;
        }
    }

    private void FlipSprite()
    {
        if (spriteRenderer == null || target == null)
            return;

        // Only flip horizontally
        if (target.position.x < transform.position.x && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }
        else if (target.position.x > transform.position.x && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void ApplyMovementConstraints()
    {
        switch (movementMode)
        {
            case MovementMode.Free:
                rb.constraints = RigidbodyConstraints2D.None;
                break;

            case MovementMode.HorizontalOnly:
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                break;

            case MovementMode.VerticalOnly:
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }

    // Method to manually toggle chase behavior if needed
    public void SetShouldChase(bool chase)
    {
        shouldChase = chase;
    }
}

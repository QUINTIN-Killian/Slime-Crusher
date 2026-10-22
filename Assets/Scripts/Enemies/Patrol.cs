using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Patrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float ledgeCheckOffset = 0.2f;
    [SerializeField] private bool turnAtLedges = true;
    [SerializeField] private LayerMask nonBlockingLayers;
    [SerializeField] private Transform rotatePointLeft = null;
    [SerializeField] private Transform rotatePointRight = null;

    public Transform RotatePointLeft => rotatePointLeft;
    public Transform RotatePointRight => rotatePointRight;

    private Vector2 moveDirection = Vector2.left;
    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private ContactFilter2D obstaclesFilter;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[4];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        obstaclesFilter.SetLayerMask(~nonBlockingLayers);
        obstaclesFilter.useTriggers = false;
    }

    private void FixedUpdate()
    {
        if (IsFacingObstacle() || IsFacingLedge() || IsRotatePointReached())
        {
            FlipDirection();
        }

        rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocityY);
    }

    public bool IsFacingObstacle(float lookAhead = 0f)
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 origin = new(bounds.center.x + moveDirection.x * bounds.extents.x, bounds.center.y);

        return HitsObstacle(origin, moveDirection, wallCheckDistance + lookAhead);
    }

    private bool IsGrounded()
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 origin = new(bounds.center.x, bounds.center.y);

        return HitsObstacle(origin, Vector2.down, bounds.extents.y + groundCheckDistance);
    }

    public bool IsFacingLedge(float lookAhead = 0f)
    {
        return turnAtLedges && IsGrounded() && IsLedgeAhead(lookAhead);
    }

    private bool IsLedgeAhead(float lookAhead)
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 origin = new(bounds.center.x + moveDirection.x * (bounds.extents.x + ledgeCheckOffset + lookAhead), bounds.center.y);

        return !HitsObstacle(origin, Vector2.down, bounds.extents.y + groundCheckDistance);
    }

    private bool IsRotatePointReached()
    {
        if (rotatePointLeft != null && IsHeadingLeft() && transform.position.x <= rotatePointLeft.position.x) return true;

        if (rotatePointRight != null && IsHeadingRight() && transform.position.x >= rotatePointRight.position.x) return true;

        return false;
    }

    private bool HitsObstacle(Vector2 origin, Vector2 direction, float distance)
    {
        int count = Physics2D.Raycast(origin, direction, obstaclesFilter, hits, distance);

        for (int i = 0; i < count; i++)
        {
            if (hits[i].collider != bodyCollider)
            {
                return true;
            }
        }
        return false;
    }

    public void FaceDirection(float direction)
    {
        if (Mathf.Approximately(direction, 0f) || Mathf.Sign(direction) == Mathf.Sign(moveDirection.x)) return;

        FlipDirection();
    }

    private void FlipDirection()
    {
        moveDirection = -moveDirection;

        Vector3 localScale = transform.localScale;

        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private bool IsHeadingLeft()
    {
        return moveDirection.x < 0;
    }

    private bool IsHeadingRight()
    {
        return moveDirection.x > 0;
    }
}

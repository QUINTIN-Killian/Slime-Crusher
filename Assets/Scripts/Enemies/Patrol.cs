using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Patrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private LayerMask nonBlockingLayers;

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
        if (IsBlocked())
        {
            FlipDirection();
        }

        rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocityY);
    }

    private bool IsBlocked()
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 origin = new(bounds.center.x + moveDirection.x * bounds.extents.x, bounds.center.y);
        int count = Physics2D.Raycast(origin, moveDirection, obstaclesFilter, hits, wallCheckDistance);

        for (int i = 0; i < count; i++)
        {
            if (hits[i].collider != bodyCollider)
            {
                return true;
            }
        }
        return false;
    }

    private void FlipDirection()
    {
        moveDirection = -moveDirection;

        Vector3 localScale = transform.localScale;

        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}

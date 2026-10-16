using UnityEngine;

[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private float dashTimeout = 3.0f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private float arrivalThreshold = 0.05f;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private LayerMask lineOfSightLayers;

    private enum DashState { Idle, Dashing, Returning }

    private Patrol patrol;
    private Rigidbody2D rb;
    private DashState state = DashState.Idle;
    private Vector2 initialPosition;
    private Vector2 currentDestination;
    private Vector3 initialScale;
    private float nextDashTime = 0f;
    private float dashTimer = 0f;
    private ContactFilter2D lineOfSightFilter;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[8];

    private void Awake()
    {
        patrol = GetComponent<Patrol>();
        rb = GetComponent<Rigidbody2D>();

        lineOfSightFilter.SetLayerMask(lineOfSightLayers);
        lineOfSightFilter.useTriggers = false;
    }

    private void FixedUpdate()
    {
        if (state == DashState.Idle) return;

        dashTimer += Time.fixedDeltaTime;

        if (dashTimer >= dashTimeout)
        {
            HandleTimeout();
            return;
        }

        if (HasReachedDestination())
        {
            if (state == DashState.Dashing)
            {
                StartReturn();
            }
            else
            {
                EndDash();
            }
            return;
        }

        float speed = state == DashState.Dashing ? dashSpeed : returnSpeed;

        rb.MovePosition(Vector2.MoveTowards(rb.position, currentDestination, speed * Time.fixedDeltaTime));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enabled || state != DashState.Idle || Time.time < nextDashTime) return;

        if (!collision.CompareTag("Player") || !collision.isTrigger) return;

        if (!HasLineOfSight(collision)) return;

        StartDash(collision.transform.position);
    }

    private void StartDash(Vector2 target)
    {
        initialPosition = rb.position;
        initialScale = transform.localScale;
        patrol.enabled = false;
        rb.linearVelocity = Vector2.zero;
        FaceTowards(target);
        HeadToPosition(target, DashState.Dashing);
    }

    private void StartReturn()
    {
        FaceTowards(initialPosition);
        HeadToPosition(initialPosition, DashState.Returning);
    }

    private void EndDash()
    {
        state = DashState.Idle;
        rb.linearVelocity = Vector2.zero;
        transform.localScale = initialScale;
        nextDashTime = Time.time + dashCooldown;
        patrol.enabled = true;
    }

    private void HeadToPosition(Vector2 position, DashState newState)
    {
        currentDestination = position;
        state = newState;
        dashTimer = 0f;
    }

    private bool HasLineOfSight(Collider2D target)
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 origin = bounds.center;
        Vector2 toTarget = (Vector2)target.bounds.center - origin;
        Vector2 direction = toTarget.normalized;
        Vector2 side = new(-direction.y, direction.x);

        return IsPathClear(origin, direction, toTarget.magnitude, target)
            && IsPathClear(GetOuterCorner(bounds, side), direction, toTarget.magnitude, target)
            && IsPathClear(GetOuterCorner(bounds, -side), direction, toTarget.magnitude, target);
    }

    private static Vector2 GetOuterCorner(Bounds bounds, Vector2 side)
    {
        return new Vector2(
            bounds.center.x + Mathf.Sign(side.x) * bounds.extents.x,
            bounds.center.y + Mathf.Sign(side.y) * bounds.extents.y);
    }

    private bool IsPathClear(Vector2 origin, Vector2 direction, float distance, Collider2D target)
    {
        int count = Physics2D.Raycast(origin, direction, lineOfSightFilter, hits, distance);
        Collider2D closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (hits[i].distance < closestDistance)
            {
                closestDistance = hits[i].distance;
                closest = hits[i].collider;
            }
        }

        return closest == null || closest.attachedRigidbody == target.attachedRigidbody;
    }

    private bool HasReachedDestination()
    {
        return (rb.position - currentDestination).sqrMagnitude <= arrivalThreshold * arrivalThreshold;
    }

    private void FaceTowards(Vector2 position)
    {
        float direction = position.x - rb.position.x;

        if (Mathf.Approximately(direction, 0f)) return;

        Vector3 localScale = transform.localScale;

        localScale.x = Mathf.Abs(initialScale.x) * -Mathf.Sign(direction);
        transform.localScale = localScale;
    }

    private void HandleTimeout()
    {
        if (state == DashState.Dashing)
        {
            StartReturn();
        }
        else if (state == DashState.Returning)
        {
            rb.position = initialPosition;
            EndDash();
        }
    }
}

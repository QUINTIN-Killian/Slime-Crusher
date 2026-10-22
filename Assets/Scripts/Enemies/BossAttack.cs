using UnityEngine;

[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2f;

    [SerializeField] private float chargeWindUpDuration = 0.8f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 3f;

    [SerializeField] private Color telegraphColor = Color.red;
    [SerializeField] private float blinkFrequency = 8f;

    private enum BossState { Idle, ChargeWindUp, Charging }

    private Patrol patrol;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BossState state = BossState.Idle;
    private Color initialColor;
    private float stateTimer = 0f;
    private float nextAttackTime = 0f;
    private float chargeDirection = 0f;

    private void Awake()
    {
        patrol = GetComponent<Patrol>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
    }

    private void OnDisable()
    {
        spriteRenderer.color = initialColor;
    }

    private void FixedUpdate()
    {
        if (state == BossState.Idle) return;

        stateTimer += Time.fixedDeltaTime;

        if (state == BossState.ChargeWindUp)
        {
            UpdateWindUp();
        }
        else
        {
            UpdateCharge();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enabled || state != BossState.Idle || Time.time < nextAttackTime) return;

        if (!collision.CompareTag("Player") || !collision.isTrigger) return;

        StartCharge(collision.transform.position);
    }

    private void StartCharge(Vector2 target)
    {
        patrol.enabled = false;
        StopHorizontalMovement();
        chargeDirection = Mathf.Sign(target.x - rb.position.x);
        patrol.FaceDirection(chargeDirection);
        ChangeState(BossState.ChargeWindUp);
    }

    private void UpdateWindUp()
    {
        StopHorizontalMovement();
        spriteRenderer.color = Mathf.Repeat(stateTimer * blinkFrequency, 1f) < 0.5f ? telegraphColor : initialColor;

        if (stateTimer >= chargeWindUpDuration)
        {
            spriteRenderer.color = initialColor;
            ChangeState(BossState.Charging);
        }
    }

    private void UpdateCharge()
    {
        if (stateTimer >= chargeDuration)
        {
            EndCharge();
            return;
        }

        float step = chargeSpeed * Time.fixedDeltaTime;
        float nextX = rb.position.x + chargeDirection * step;

        if (patrol.IsFacingObstacle(step) || patrol.IsFacingLedge(step))
        {
            TurnAround();
        }
        else if (TryGetChargeLimit(out float limitX) && (nextX - limitX) * chargeDirection >= 0f)
        {
            rb.position = new Vector2(limitX, rb.position.y);
            TurnAround();
        }

        rb.linearVelocity = new Vector2(chargeDirection * chargeSpeed, rb.linearVelocityY);
    }

    private void TurnAround()
    {
        chargeDirection = -chargeDirection;
        patrol.FaceDirection(chargeDirection);
    }

    private void EndCharge()
    {
        ChangeState(BossState.Idle);
        StopHorizontalMovement();
        spriteRenderer.color = initialColor;
        nextAttackTime = Time.time + attackCooldown;
        patrol.enabled = true;
    }

    private void ChangeState(BossState newState)
    {
        state = newState;
        stateTimer = 0f;
    }

    private void StopHorizontalMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocityY);
    }

    private bool TryGetChargeLimit(out float limitX)
    {
        Transform limit = chargeDirection < 0 ? patrol.RotatePointLeft : patrol.RotatePointRight;

        limitX = limit != null ? limit.position.x : 0f;
        return limit != null;
    }
}

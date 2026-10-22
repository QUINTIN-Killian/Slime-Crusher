using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Score))]
public class EnemyHealth : ADamageable
{
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    [SerializeField] private float deathGravityScale = 1f;

    private const float DespawnTime = 0.3f;
    private Animator anim;
    private Rigidbody2D rb;
    private Score score;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        score = GetComponent<Score>();
    }

    protected override void OnHit()
    {
        anim.SetTrigger(HitHash);
    }

    protected override void Die()
    {
        anim.SetTrigger(DeathHash);
        rb.linearVelocity = new Vector2(0f, Mathf.Min(rb.linearVelocityY, 0f));
        rb.gravityScale = deathGravityScale;
        score.DeclareScore();
    }

    public void OnDespawnAnimationEnd()
    {
        Destroy(gameObject, DespawnTime);
    }
}

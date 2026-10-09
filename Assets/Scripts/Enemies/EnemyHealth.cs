using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Score))]
public class EnemyHealth : ADamageable
{
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    private const float DespawnTime = 0.3f;
    private Animator anim;
    private Score score;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        score = GetComponent<Score>();
    }

    protected override void OnHit()
    {
        anim.SetTrigger(HitHash);
    }

    protected override void Die()
    {
        anim.SetTrigger(DeathHash);
        GetComponent<Patrol>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        score.DeclareScore();
    }

    public void OnDespawnAnimationEnd()
    {
        Destroy(gameObject, DespawnTime);
    }
}

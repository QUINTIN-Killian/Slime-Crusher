using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : ADamageable
{
    private const float DespawnTime = 0.3f;
    private Animator anim;
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    private void Awake()
    {
        anim = GetComponent<Animator>();
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
    }

    public void OnDespawnAnimationEnd()
    {
        Destroy(gameObject, DespawnTime);
    }
}

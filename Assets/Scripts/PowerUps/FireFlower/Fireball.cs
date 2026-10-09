using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Fireball : AThrowable
{
    private static readonly int ExplodeHash = Animator.StringToHash("Explode");

    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected override void OnImpact()
    {
        anim.SetTrigger(ExplodeHash);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamage(collision.collider);
        Impact();
    }

    public void OnExplodeAnimationEnd()
    {
        Destroy(gameObject);
    }
}

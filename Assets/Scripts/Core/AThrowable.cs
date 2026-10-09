using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class AThrowable : MonoBehaviour
{
    [SerializeField] protected float speed = 10.0f;
    [SerializeField] protected float lifetime = 2.0f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected AudioClip impactSound;

    public event Action<AThrowable> Destroyed;

    protected Rigidbody2D rb;
    protected float direction;
    protected bool HasImpacted { get; private set; } = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float dir)
    {
        direction = Mathf.Sign(dir);
        OnLaunch();
        StartCoroutine(LifetimeHandling());
    }

    private IEnumerator LifetimeHandling()
    {
        yield return new WaitForSeconds(lifetime);
        Impact();
    }

    protected void Impact()
    {
        if (HasImpacted) return;

        HasImpacted = true;
        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position, 1.0f);
        }

        OnImpact();
    }

    protected bool TryDamage(Collider2D other)
    {
        ADamageable target = other.GetComponentInParent<ADamageable>();

        if (target == null) return false;

        target.TakeDamage(damage);
        return true;
    }

    protected virtual void OnLaunch()
    {
        rb.linearVelocity = new Vector2(direction * speed, 0f);
    }

    protected virtual void OnImpact()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }
}

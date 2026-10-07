using UnityEngine;

public abstract class ADamageable : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected AudioClip hitSound = null;
    [SerializeField] protected AudioClip deathSound = null;

    public bool IsDead { get; private set; } = false;

    public void TakeDamage(int amount = 1)
    {
        if (IsDead) return;

        healthPoints = Mathf.Max(healthPoints - amount, 0);
        ApplyHealthChange();
    }

    public void Kill()
    {
        if (IsDead) return;

        healthPoints = 0;
        ApplyHealthChange();
    }

    private void ApplyHealthChange()
    {
        if (healthPoints <= 0)
        {
            IsDead = true;
            Die();

            if (deathSound)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position, 1.0f);
            }
        }
        else
        {
            OnHit();

            if (hitSound)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position, 1.0f);
            }
        }

        OnHealthChanged();
    }

    protected virtual void OnHit()
    {
    }

    protected virtual void OnHealthChanged()
    {
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;

public abstract class ADamageable : MonoBehaviour
{
    private const string DeadLayerName = "DeadEntity";

    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected AudioClip hitSound = null;
    [SerializeField] protected AudioClip deathSound = null;
    [SerializeField] private Behaviour[] disabledOnDeath;

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
            DisableBehaviours();
            SetLayerRecursively(transform, LayerMask.NameToLayer(DeadLayerName));
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

    private void DisableBehaviours()
    {
        foreach (Behaviour behaviour in disabledOnDeath)
        {
            behaviour.enabled = false;
        }
    }

    private static void SetLayerRecursively(Transform target, int layer)
    {
        target.gameObject.layer = layer;

        foreach (Transform child in target)
        {
            SetLayerRecursively(child, layer);
        }
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

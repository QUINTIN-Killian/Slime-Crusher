using UnityEngine;
using System.Collections;

public abstract class ADamageable : MonoBehaviour
{
    private const string DeadLayerName = "DeadEntity";

    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected AudioClip hitSound = null;
    [SerializeField] protected AudioClip deathSound = null;
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private Renderer blinkRenderer;
    [SerializeField] private Behaviour[] disabledOnDeath;

    private Coroutine invincibilityRoutine;

    public bool IsDead { get; private set; } = false;
    public bool IsInvincible => invincibilityRoutine != null;

    public void TakeDamage(int amount = 1)
    {
        if (IsInvincible || IsDead) return;

        healthPoints = Mathf.Max(healthPoints - amount, 0);
        ApplyHealthChange();

        if (!IsDead)
        {
            invincibilityRoutine = StartCoroutine(InvincibilityRoutine());
        }
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
            StopInvincibility();
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

    private IEnumerator InvincibilityRoutine()
    {
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            SetRendererVisible(!IsRendererVisible());
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        StopInvincibility();
    }

    private void StopInvincibility()
    {
        if (invincibilityRoutine != null)
        {
            StopCoroutine(invincibilityRoutine);
            invincibilityRoutine = null;
        }

        SetRendererVisible(true);
    }

    private bool IsRendererVisible()
    {
        return !blinkRenderer || blinkRenderer.enabled;
    }

    private void SetRendererVisible(bool visible)
    {
        if (blinkRenderer)
        {
            blinkRenderer.enabled = visible;
        }
    }

    private void OnDisable()
    {
        StopInvincibility();
    }
}

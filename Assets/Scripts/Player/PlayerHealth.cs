using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerHealth : ADamageable
{
    public event Action<int> HealthChanged;
    public event Action Died;
    private Animator anim;
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");
    private PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        HealthChanged?.Invoke(healthPoints);
    }

    protected override void OnHit()
    {
        anim.SetTrigger(HitHash);
    }

    protected override void OnHealthChanged()
    {
        HealthChanged?.Invoke(healthPoints);
    }

    protected override void Die()
    {
        anim.SetTrigger(DeathHash);
        playerMovement.SetFrozen(true);
    }

    public void OnDeathAnimationEnd()
    {
        Died?.Invoke();
    }
}

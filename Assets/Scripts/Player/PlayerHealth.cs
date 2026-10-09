using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerPowerUpsHandler))]
public class PlayerHealth : ADamageable
{
    public event Action<int> HealthChanged;
    public event Action Died;

    private Animator anim;
    private PlayerPowerUpsHandler playerPickup;
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeathHash = Animator.StringToHash("Death");
    private PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerPickup = GetComponent<PlayerPowerUpsHandler>();
    }

    private void Start()
    {
        HealthChanged?.Invoke(healthPoints);
    }

    protected override void OnHit()
    {
        anim.SetTrigger(HitHash);
        playerPickup.RemovePowerUp();
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

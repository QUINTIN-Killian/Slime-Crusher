using UnityEngine;
using System;

public class BossDeath : MonoBehaviour
{
    public event Action BossKilled;

    public void OnDespawnAnimationEnd()
    {
        BossKilled?.Invoke();
    }
}

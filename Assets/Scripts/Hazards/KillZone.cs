using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ADamageable damageable = other.GetComponentInParent<ADamageable>();

        if (damageable != null)
        {
            damageable.Kill();
        }
    }
}

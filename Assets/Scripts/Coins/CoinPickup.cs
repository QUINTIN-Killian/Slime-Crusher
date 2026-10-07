using UnityEngine;
using System;

public class CoinPickup : MonoBehaviour
{
    public static event Action<int> Collected;

    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, 1.0f);
            }

            Collected?.Invoke(value);
            Destroy(gameObject);
        }
    }
}

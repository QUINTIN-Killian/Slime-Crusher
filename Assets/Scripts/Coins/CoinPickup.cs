using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !other.isTrigger) return;

        CoinWallet wallet = other.GetComponentInParent<CoinWallet>();

        if (wallet == null) return;

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 1.0f);
        }

        wallet.AddCoins(value);
        Destroy(gameObject);
    }
}

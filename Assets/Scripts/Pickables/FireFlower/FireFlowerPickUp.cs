using UnityEngine;

public class FireFlowerPickUp : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponentInParent<PlayerPickup>().HandlePowerUpPickup("fire_flower");
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 1.0f);
            Destroy(gameObject);
        }
    }
}

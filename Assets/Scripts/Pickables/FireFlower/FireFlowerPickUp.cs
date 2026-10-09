using UnityEngine;

public class FireFlowerPickUp : MonoBehaviour
{
    [SerializeField] private string pickupTag = "fire_flower";
    [SerializeField] private string animationTrigger = "ThrowFireball";
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip removeSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponentInParent<PlayerPowerUpsHandler>().EquipPowerUp(pickupTag, animationTrigger, pickupSound, removeSound);
            Destroy(gameObject);
        }
    }
}

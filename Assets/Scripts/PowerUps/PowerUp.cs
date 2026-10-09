using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private string pickupTag;
    [SerializeField] private string animationTrigger;
    [SerializeField] private AThrowable throwable;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip removeSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponentInParent<PlayerPowerUpsHandler>().EquipPowerUp(pickupTag, animationTrigger, throwable, pickupSound, removeSound);
            Destroy(gameObject);
        }
    }
}

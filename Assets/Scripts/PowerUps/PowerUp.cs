using UnityEngine;

[RequireComponent(typeof(Score))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private string pickupTag;
    [SerializeField] private string animationTrigger;
    [SerializeField] private AThrowable throwable;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip removeSound;

    private Score score;

    private void Awake()
    {
        score = GetComponent<Score>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponentInParent<PlayerPowerUpsHandler>().EquipPowerUp(pickupTag, animationTrigger, throwable, pickupSound, removeSound);
            score.DeclareScore();
            Destroy(gameObject);
        }
    }
}

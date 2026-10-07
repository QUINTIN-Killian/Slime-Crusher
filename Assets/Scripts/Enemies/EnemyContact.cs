using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyContact : MonoBehaviour
{
    [SerializeField] private float bounceForce = 8f;

    private BoxCollider2D hitbox;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider2D>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !other.isTrigger) return;

        Rigidbody2D playerRb = other.attachedRigidbody;
        bool isFalling = playerRb.linearVelocity.y < 0f;
        bool isAbove = other.bounds.min.y > hitbox.bounds.center.y;

        if (isFalling && isAbove)
        {
            enemyHealth.TakeDamage();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
        }
        else
        {
            other.GetComponentInParent<PlayerHealth>().TakeDamage();
        }
    }
}

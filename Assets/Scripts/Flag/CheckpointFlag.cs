using UnityEngine;
using System;

[RequireComponent(typeof(Score))]
public class CheckpointFlag : MonoBehaviour
{
    public static event Action<Vector3> Reached;

    [SerializeField] private Confetti[] confettis;
    [SerializeField] private AudioClip reachSound;

    private bool validated = false;
    private Score score;

    private void Awake()
    {
        score = GetComponent<Score>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger && !validated)
        {
            validated = true;
            Reached?.Invoke(transform.position);

            if (reachSound != null)
            {
                AudioSource.PlayClipAtPoint(reachSound, transform.position, 1.0f);
            }

            foreach (Confetti confetti in confettis) confetti.Burst();

            score.DeclareScore();
        }
    }
}

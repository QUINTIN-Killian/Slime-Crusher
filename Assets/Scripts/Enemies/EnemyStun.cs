using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyStun : MonoBehaviour, IStunnable
{
    [SerializeField] private Behaviour[] disabledWhileStunned;

    private Coroutine stunCoroutine;
    private Rigidbody2D rb;
    private ADamageable damageable;
    private bool[] wasEnabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<ADamageable>();
        wasEnabled = new bool[disabledWhileStunned.Length];
    }

    public void Stun(float time)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        else
        {
            DisableBehaviours();
        }

        stunCoroutine = StartCoroutine(StunHandling(time));
    }

    private IEnumerator StunHandling(float time)
    {
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(time);

        stunCoroutine = null;

        if (damageable != null && damageable.IsDead) yield break;

        RestoreBehaviours();
        rb.linearVelocity = Vector2.zero;
    }

    private void DisableBehaviours()
    {
        for (int i = 0; i < disabledWhileStunned.Length; i++)
        {
            wasEnabled[i] = disabledWhileStunned[i].enabled;
            disabledWhileStunned[i].enabled = false;
        }
    }

    private void RestoreBehaviours()
    {
        for (int i = 0; i < disabledWhileStunned.Length; i++)
        {
            disabledWhileStunned[i].enabled = wasEnabled[i];
        }
    }
}

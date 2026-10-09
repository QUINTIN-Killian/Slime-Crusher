using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyStun : MonoBehaviour, IStunnable
{
    private Patrol patrol;
    private Coroutine stunCoroutine;
    private Rigidbody2D rb;

    private void Awake()
    {
        patrol = GetComponent<Patrol>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Stun(float time)
    {
        if (stunCoroutine != null) StopCoroutine(stunCoroutine);

        stunCoroutine = StartCoroutine(StunHandling(time));
    }

    private IEnumerator StunHandling(float time)
    {
        patrol.enabled = false;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(time);

        patrol.enabled = true;
        rb.linearVelocity = Vector2.zero;
    }
}

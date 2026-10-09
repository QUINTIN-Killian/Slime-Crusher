using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Animator))]
public class Confetti : MonoBehaviour
{
    private static readonly int BurstHash = Animator.StringToHash("burst");

    private Renderer renderer;
    private Animator anim;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        renderer.enabled = false;
    }

    public void Burst()
    {
        renderer.enabled = true;
        anim.Play(BurstHash);
    }

    public void OnBurstAnimationEnd()
    {
        Destroy(gameObject);
    }
}

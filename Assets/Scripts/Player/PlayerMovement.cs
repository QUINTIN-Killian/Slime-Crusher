using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerPowerUpsHandler))]
public class PlayerMovement : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private ContactFilter2D groundFilter;
    private PlayerPowerUpsHandler playerPickup;
    private float horizontalInput;
    private bool jumpRequested = false;
    private bool frozen = false;
    private Vector3 initialScale;
    private Vector3 powerUpScale;

    private void OnEnable()
    {
        gameManager.GameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        gameManager.GameEnded -= HandleGameEnded;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerPickup = GetComponent<PlayerPowerUpsHandler>();

        groundFilter = new ContactFilter2D();
        groundFilter.SetLayerMask(groundLayers);
        groundFilter.SetNormalAngle(45f, 135f);
    }

    private void Start()
    {
        initialScale = transform.localScale;
        powerUpScale = initialScale * scaleMultiplier;
    }

    private void Update()
    {
        ReadInputs();
        UpdateAnimations();

        if (HasPowerUp() && IsInitialScale()) transform.localScale = powerUpScale;
        else if (!HasPowerUp() && !IsInitialScale()) transform.localScale = initialScale;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;

        HandleHorizontalMovement(ref velocity);
        HandleJump(ref velocity);
        rb.linearVelocity = velocity;
    }

    public void SetFrozen(bool frozen)
    {
        this.frozen = frozen;
    }

    private void ReadInputs()
    {
        if (frozen)
        {
            horizontalInput = 0f;
            jumpRequested = false;
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetFloat(SpeedHash, Mathf.Abs(horizontalInput));

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void HandleHorizontalMovement(ref Vector2 velocity)
    {
        velocity.x = horizontalInput * speed;
    }

    private void HandleJump(ref Vector2 velocity)
    {
        if (jumpRequested && IsGrounded())
        {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position, 1.0f);
            anim.SetTrigger(JumpHash);
            velocity.y = jumpForce;
        }

        jumpRequested = false;
    }

    private bool IsGrounded()
    {
        return rb.IsTouching(groundFilter);
    }

    private void HandleGameEnded(bool hasWon)
    {
        SetFrozen(true);
    }

    private bool HasPowerUp()
    {
        return playerPickup.HasPowerUp;
    }

    private bool IsInitialScale()
    {
        return transform.localScale == initialScale;
    }
}

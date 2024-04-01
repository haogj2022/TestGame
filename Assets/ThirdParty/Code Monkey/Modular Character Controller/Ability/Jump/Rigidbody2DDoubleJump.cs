using UnityEngine;

public class Rigidbody2DDoubleJump : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    [SerializeField] private float doubleJumpPower;
    [SerializeField] private LayerMask groundLayer;
    public AudioManager audioManager;

    private Animator anim;
    private Rigidbody2D rb2D;
    private Transform groundCheck;
    private bool canDoubleJump;
    //private float fallMultiplier = 1f;
    //private float lowJumpMultiplier = 1f;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter;

    private ParticleSystem dust;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        groundCheck = new GameObject("Ground Check").transform;
        groundCheck.parent = transform;
        groundCheck.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

        dust = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        anim.SetBool("Jump", false);        

        if (!anim.GetBool("Tunnel") && !anim.GetBool("Crouch"))
        {
            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                anim.SetBool("Jump", true);
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferTimeCounter -= Time.deltaTime;
            }

            if (!Input.GetButton("Jump") && IsGrounded())
            {
                canDoubleJump = false;
            }

            if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f || jumpBufferTimeCounter > 0f && canDoubleJump)
            {
                audioManager.Jump();
                CreateDust();
                anim.SetBool("Jump", true);
                rb2D.velocity = new Vector2(rb2D.velocity.x, canDoubleJump ? doubleJumpPower : jumpPower);
                //rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
                canDoubleJump = !canDoubleJump;

                jumpBufferTimeCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0f)
            {
                anim.SetBool("Jump", true);
                rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);
                //rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;

                coyoteTimeCounter = 0f;
            }
        }        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void CreateDust()
    {
        dust.Play();
    }
}

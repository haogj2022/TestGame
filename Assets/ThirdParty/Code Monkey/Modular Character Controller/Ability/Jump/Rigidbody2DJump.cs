using UnityEngine;

public class Rigidbody2DJump : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;

    private Animator anim;
    private Rigidbody2D rb2D;
    private Transform groundCheck;    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        groundCheck = new GameObject("Ground Check").transform;        
        groundCheck.parent = transform;
        groundCheck.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
    }

    private void Update()
    {
        anim.SetBool("Jump", false);

        if (!IsGrounded())
        {
            anim.SetBool("Jump", true);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            anim.SetBool("Jump", true);
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb2D.velocity.y > 0f)
        {
            anim.SetBool("Jump", true);
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * 0.5f);
        }        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}

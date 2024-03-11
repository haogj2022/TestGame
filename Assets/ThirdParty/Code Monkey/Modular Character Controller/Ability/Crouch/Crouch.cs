using UnityEngine;

public class Crouch : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    private BoxCollider2D boxCollider2D;
    private Transform groundCheckUp;
    private Transform groundCheckDown;
    private Animator anim;

    private bool canCrouch = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheckUp = new GameObject("Ground Check Up").transform;
        groundCheckUp.parent = transform;
        groundCheckUp.position = new Vector2(transform.position.x, transform.position.y + 0.25f);
        groundCheckDown = new GameObject("Ground Check Down").transform;
        groundCheckDown.parent = transform;
        groundCheckDown.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
        {
            canCrouch = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
        {
            canCrouch = true;
        }
    }

    private void Update()
    {
        if (!anim.GetBool("Fly") && !anim.GetBool("Tunnel"))
        {
            if (Input.GetKeyDown(KeyCode.S) && IsGroundedDown() && canCrouch || Input.GetKeyDown(KeyCode.DownArrow) && IsGroundedDown() && canCrouch)
            {
                anim.SetBool("Crouch", true);
                boxCollider2D.offset = new Vector2(0f, -0.125f);
                boxCollider2D.size = new Vector2(0.5f, 0.25f);                
            }

            if (Input.GetKeyDown(KeyCode.W) && !IsGroundedUp() || Input.GetKeyDown(KeyCode.UpArrow) && !IsGroundedUp() || !IsGroundedDown())
            {
                anim.SetBool("Crouch", false);
                boxCollider2D.offset = Vector2.zero;
                boxCollider2D.size = new Vector2(0.5f, 0.5f);
            }
        }        
    }

    private bool IsGroundedUp()
    {
        return Physics2D.OverlapCircle(groundCheckUp.position, 0.2f, groundLayer);
    }

    private bool IsGroundedDown()
    {
        return Physics2D.OverlapCircle(groundCheckDown.position, 0.2f, groundLayer);
    }
}

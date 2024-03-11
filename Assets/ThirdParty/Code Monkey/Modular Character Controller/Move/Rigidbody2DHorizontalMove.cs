using System.Collections;
using UnityEngine;

public class Rigidbody2DHorizontalMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private float horizontalFloat;
    private Rigidbody2D rb2D;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.3f;
    private float dashingCool = 1f;
    private TrailRenderer tr;
    private Animator anim;

    private bool wallDash;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "No Dash")
        {
            canDash = false;
        }

        if (collision.tag == "Wall Dash")
        {
            wallDash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "No Dash")
        {
            canDash = true;
        }

        if (collision.tag == "Wall Dash")
        {
            wallDash = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            if (!anim.GetBool("Tunnel") && !anim.GetBool("Fly") && !anim.GetBool("Crouch"))
            {
                StartCoroutine(Dash());
            }          
        }

        if (!isDashing && !anim.GetBool("Fly"))
        {
            horizontalFloat = Input.GetAxisRaw("Horizontal");
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed, rb2D.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        Flip();          
    }

    private void Flip()
    {
        if (horizontalFloat > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (horizontalFloat < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    private IEnumerator Dash()
    {
        anim.SetBool("Dash", true);
        canDash = false;
        isDashing = true;
        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;

        if (transform.rotation.y >= 0f)
        {
            rb2D.velocity = new Vector2(dashingPower, 0f);

            if (Input.GetButton("Jump") && horizontalFloat > 0f)
            {
                rb2D.velocity = new Vector2(dashingPower / 1.5f, dashingPower / 1.5f);
                transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            }            
        }
        else
        {
            rb2D.velocity = new Vector2(-dashingPower, 0f);

            if (Input.GetButton("Jump") && horizontalFloat < 0f)
            {
                rb2D.velocity = new Vector2(-dashingPower / 1.5f, dashingPower / 1.5f);
                transform.rotation = Quaternion.Euler(0f, 180f, 45f);
            }            
        }

        if (wallDash)
        {
            anim.SetBool("Dash", false);
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        anim.SetBool("Dash", false);
        tr.emitting = false;
        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCool);
        canDash = true;
    }
}

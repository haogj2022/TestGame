using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rigidbody2DHorizontalMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;   

    private float horizontalFloat;
    private Rigidbody2D rb2D;
    public AudioManager audioManager;
    public Slider dashBar;

    [HideInInspector] public bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.3f;
    private float dashingCool = 1f;
    private TrailRenderer tr;
    private Animator anim;
    private ParticleSystem dust;

    private bool wallDash;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        dust = GetComponentInChildren<ParticleSystem>();
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

        if (collision.tag == "Water")
        {
            if (horizontalFloat > 0f)
            {
                Flip(true);
            }
            else
            {
                Flip(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            if (!anim.GetBool("Tunnel") && !anim.GetBool("Fly") && !anim.GetBool("Crouch"))
            {
                dashBar.value = dashBar.maxValue;
                dashBar.gameObject.SetActive(true);
                audioManager.Dash();
                StartCoroutine(Dash());
            }          
        }

        if (!isDashing && !anim.GetBool("Fly"))
        {
            horizontalFloat = Input.GetAxisRaw("Horizontal");
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed, rb2D.velocity.y);
        }

        if (horizontalFloat > 0f && !isFacingRight)
        {
            if (rb2D.velocity.y == 0f)
            {
                CreateDust();
            }

            Flip(true);
        }
        else if (horizontalFloat < 0f && isFacingRight)
        {
            if (rb2D.velocity.y == 0f)
            {
                CreateDust();
            }

            Flip(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeLeft")
        {
            Flip(true);
        }

        if (collision.gameObject.tag == "LedgeRight")
        {
            Flip(false);
        }
    }

    public void Dead()
    {
        anim.SetBool("Dash", false);
        tr.emitting = false;
        rb2D.gravityScale = 1f;
        isDashing = false;
        canDash = true;
    }

    public void Flip(bool right)
    {
        isFacingRight = right;
        transform.rotation = Quaternion.Euler(0f, right ? 0f : 180f, 0f);
    }

    public IEnumerator Dash()
    {
        anim.SetBool("Dash", true);
        canDash = false;
        isDashing = true;
        float originalGravity = rb2D.gravityScale;
        rb2D.gravityScale = 0f;
        tr.emitting = true;

        if (transform.rotation.y >= 0f)
        {
            rb2D.velocity = new Vector2(dashingPower, 0f);
        }
        else
        {
            rb2D.velocity = new Vector2(-dashingPower, 0f);
        }

        if (wallDash)
        {
            anim.SetBool("Dash", false);
        }

        yield return new WaitForSeconds(dashingTime);
        anim.SetBool("Dash", false);
        tr.emitting = false;
        rb2D.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCool);
        canDash = true;
    }

    public void CreateDust()
    {
        if (!anim.GetBool("Tunnel"))
        {
            dust.Play();
        }        
    }
}

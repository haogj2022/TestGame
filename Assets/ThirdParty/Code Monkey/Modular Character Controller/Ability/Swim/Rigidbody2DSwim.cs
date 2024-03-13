using System.Collections;
using UnityEngine;

public class Rigidbody2DSwim : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator swimControl;

    private float horizontalFloat;
    private float verticalFloat;
    private Rigidbody2D rb2D;
    private Animator anim;
    private TrailRenderer tr;

    private float thrust = 2f;
    private bool canDash = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            swimControl.SetBool("Left", true);
            StartCoroutine(SwimAbility());
        }
    }

    IEnumerator SwimAbility()
    {
        yield return new WaitForSeconds(3f);
        swimControl.SetBool("Left", false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            anim.SetBool("Swim", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            anim.SetBool("Swim", false);
        }
    }

    private void FixedUpdate()
    {
        horizontalFloat = Input.GetAxis("Horizontal");
        verticalFloat = Input.GetAxis("Vertical");

        if (anim.GetBool("Swim"))
        {
            anim.SetFloat("Velocity", 0f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed, verticalFloat * moveSpeed);

            SwimDirection();

            if (Input.GetKey(KeyCode.Space) && canDash)
            {
                rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
                StartCoroutine(WaterDash());
            }
        }
    }

    IEnumerator WaterDash()
    {
        tr.emitting = true;
        yield return new WaitForSeconds(0.3f);
        tr.emitting = false;
        canDash = false;
        yield return new WaitForSeconds(1f);
        canDash = true;
    }

    private void Update()
    {
        if (anim.GetBool("Swim"))
        {
            SwimDirection();         
        }
    }

    private void SwimDirection()
    {
        SwimHorizontally();
        SwimVertically();
        SwimDiagonally();
    }

    private void SwimHorizontally()
    {
        if (horizontalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);      
        }

        if (horizontalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }

    private void SwimVertically()
    {
        if (verticalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (verticalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void SwimDiagonally()
    {
        if (verticalFloat > 0f && horizontalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 315f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed / 1.5f, verticalFloat * moveSpeed / 1.5f);
        }

        if (verticalFloat < 0f && horizontalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 225f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed / 1.5f, verticalFloat * moveSpeed / 1.5f);
        }

        if (verticalFloat > 0f && horizontalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed / 1.5f, verticalFloat * moveSpeed / 1.5f);
        }

        if (verticalFloat < 0f && horizontalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            transform.rotation = Quaternion.Euler(0f, 0f, 135f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed / 1.5f, verticalFloat * moveSpeed / 1.5f);
        }
    }
}

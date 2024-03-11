using System.Collections;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    private float horizontalFloat;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        horizontalFloat = Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
        {
            Physics2D.gravity = new Vector2(0f, 9.81f);
            transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            StartCoroutine(FallOffLedge());
        }        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeLeft")
        {
            anim.SetBool("Dash", false);
            Physics2D.gravity = new Vector2(-9.81f, 0f);
            transform.rotation = Quaternion.Euler(180f, 0f, -90f);
        }

        if (collision.gameObject.tag == "LedgeRight")
        {
            anim.SetBool("Dash", false);
            Physics2D.gravity = new Vector2(9.81f, 0f);
            transform.rotation = Quaternion.Euler(180f, 180f, -90f);
        }      
        
        if (collision.gameObject.tag == "LedgeUp")
        {
            if (horizontalFloat > 0f)
            {
                transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            }

            if (horizontalFloat < 0f)
            {
                transform.rotation = Quaternion.Euler(180f, 180f, 0f);
            }               
        }
    }

    IEnumerator FallOffLedge()
    {
        yield return new WaitForSeconds(3f);        
        Physics2D.gravity = new Vector2(0f, -9.81f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            StopAllCoroutines();
        }

        if (collision.gameObject.tag == "LedgeLeft")
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (collision.gameObject.tag == "LedgeRight")
        {
            Physics2D.gravity = new Vector2(0f, -9.81f);
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }        
    }
}

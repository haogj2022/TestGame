using System.Collections;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    public Animator dropDownControl;

    private float horizontalFloat;
    private Animator anim;

    private Rigidbody2DDoubleJump rb2DDoubleJump;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2DDoubleJump = GetComponent<Rigidbody2DDoubleJump>();
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

            if (transform.rotation.y >= 0f)
            {
                transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(180f, 180f, 0f);
            }
        }        
    }

    IEnumerator AutoDropDown()
    {
        yield return new WaitForSeconds(3f);

        rb2DDoubleJump.canJump = false;

        Physics2D.gravity = new Vector2(0f, -9.81f);

        if (transform.rotation.y >= 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    IEnumerator ShowDropDownControl()
    {
        dropDownControl.SetBool("Left", true);
        yield return new WaitForSeconds(3f);
        dropDownControl.SetBool("Left", false);
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
            StartCoroutine(ShowDropDownControl());
            StartCoroutine(AutoDropDown());
            Physics2D.gravity = new Vector2(0f, 9.81f);
            transform.rotation = Quaternion.Euler(180f, transform.rotation.y, 0f);

            if (horizontalFloat > 0f)
            {              
                transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            }

            if (horizontalFloat < 0f)
            {
                transform.rotation = Quaternion.Euler(180f, 180f, 0f);
            }         
            
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dropDownControl.SetBool("Left", false);

                Physics2D.gravity = new Vector2(0f, -9.81f);

                if (transform.rotation.y >= 0f)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
        {
            StopAllCoroutines();

            rb2DDoubleJump.canJump = false;

            Physics2D.gravity = new Vector2(0f, -9.81f);

            if (transform.rotation.y >= 0f)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
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

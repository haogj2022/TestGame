using System.Collections;
using UnityEngine;

public class Rigidbody2DFly : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private Animator batControl;

    private float horizontalFloat;
    private float verticalFloat;
    private Rigidbody2D rb2D;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bat" && !anim.GetBool("Fly"))
        {
            anim.SetBool("Fly", true);
            batControl.SetBool("Left", true);
            StartCoroutine(FlyAbility());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    IEnumerator FlyAbility()
    {
        StartCoroutine(TransformEffect());
        yield return new WaitForSeconds(3f);
        anim.SetBool("Fly", false);
        batControl.SetBool("Left", false);
        StartCoroutine(TransformEffect());
    }

    public IEnumerator TransformEffect()
    {
        smokeEffect.transform.position = transform.position;
        smokeEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        smokeEffect.SetActive(false);
    }

    private void Update()
    {
        horizontalFloat = Input.GetAxisRaw("Horizontal");
        verticalFloat = Input.GetAxisRaw("Vertical");

        if (anim.GetBool("Fly"))
        {
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed, verticalFloat * moveSpeed);
        }         
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("Swim"))
        {
            anim.SetFloat("Swim Velocity", 0f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed / 2, verticalFloat * moveSpeed / 2);

            if (horizontalFloat > 0f)
            {
                anim.SetFloat("Swim Velocity", 1f);
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }

            if (horizontalFloat < 0f)
            {
                anim.SetFloat("Swim Velocity", 1f);
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }

            if (verticalFloat > 0f)
            {
                anim.SetFloat("Swim Velocity", 1f);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (verticalFloat < 0f)
            {
                anim.SetFloat("Swim Velocity", 1f);
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
    }
}

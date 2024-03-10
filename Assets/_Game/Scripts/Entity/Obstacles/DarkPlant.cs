using UnityEngine;

public class DarkPlant : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    private Animator anim;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().gotKey)
            {
                hint.SetActive(false);
                collision.gameObject.GetComponent<PlayerController>().DropKey();
                anim.SetBool("Open", true);
            }
            else
            {
                hint.transform.position = transform.position;
                hint.SetActive(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hint.SetActive(false);
        }
    }

    public void Open()
    {      
        boxCollider2D.enabled = false;
    }

    public void Close()
    {      
        boxCollider2D.enabled = true;
        anim.SetBool("Open", false);
    }
}

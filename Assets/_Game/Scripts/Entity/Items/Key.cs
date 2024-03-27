using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 dropKey;

    [SerializeField] private GameObject promptText;

    [SerializeField] private bool bossKey;

    [SerializeField] private bool enemyDrop;

    [SerializeField] Animator parryControl;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!bossKey && !enemyDrop)
            {
                promptText.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                promptText.SetActive(true);
            }

            if (bossKey)
            {
                collision.gameObject.GetComponent<PlayerController>().GotKey();
                PickUpKey(collision);
            }

            if (enemyDrop)
            {
                collision.gameObject.GetComponent<PlayerController>().GotSword();
                PickUpKey(collision);
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (!bossKey)
                {
                    StartCoroutine(DespawnKey());
                }                
            }
        }
    }

    private void PickUpKey(Collision2D collision)
    {
        transform.rotation = collision.transform.rotation;
        transform.parent = collision.transform;
        transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y + 0.5f);

        if (enemyDrop && transform.rotation.y >= 0f)
        {
            transform.position = new Vector2(collision.transform.position.x + 0.25f, collision.transform.position.y + 0.05f);
            StartCoroutine(ParryControl());
        }

        if (enemyDrop && transform.rotation.y < 0f)
        {
            transform.position = new Vector2(collision.transform.position.x - 0.25f, collision.transform.position.y + 0.05f);
            StartCoroutine(ParryControl());
        }

        boxCollider2D.isTrigger = true;
    }

    IEnumerator ParryControl()
    {
        parryControl.SetBool("Left", true);
        yield return new WaitForSeconds(3f);
        parryControl.SetBool("Left", false);
    }

    IEnumerator DespawnKey()
    {
        yield return new WaitForSeconds(3f);
        DropKey();
    }

    public void DropKey()
    {
        if (bossKey)
        {
            gameObject.SetActive(true);
        }

        transform.parent = null;
        transform.position = dropKey;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        boxCollider2D.isTrigger = false;
        StopAllCoroutines();
    }

    public void DropSword()
    {
        if (enemyDrop)
        {
            gameObject.SetActive(true);
        }

        transform.parent = null;
        transform.position = dropKey;
        transform.rotation = Quaternion.Euler(0f, 0f, 45f);
        boxCollider2D.isTrigger = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!bossKey && !enemyDrop)
            {
                promptText.SetActive(false);
            }            
        }
    }  
}

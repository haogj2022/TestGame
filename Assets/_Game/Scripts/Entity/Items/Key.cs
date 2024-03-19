using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 dropKey;

    [SerializeField] private GameObject promptText;

    [SerializeField] private bool bossKey;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!bossKey)
            {
                promptText.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                promptText.SetActive(true);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().GotKey();
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
        promptText.SetActive(false);       
        transform.parent = collision.transform;
        transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y + 0.5f);
        boxCollider2D.isTrigger = true;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            promptText.SetActive(false);
        }
    }  
}

using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 dropKey;

    [SerializeField] private GameObject promptText;

    [SerializeField] private bool bossKey;

    [SerializeField] private bool silverSword;

    [SerializeField] private bool goldSword;

    [SerializeField] Animator parryControl;

    public AudioManager audioManager;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!bossKey && !silverSword && !goldSword)
            {
                promptText.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                promptText.SetActive(true);
            }

            if (bossKey)
            {
                collision.gameObject.GetComponent<PlayerController>().GotKey();
                PickUpKey(collision);
            }

            if (silverSword)
            {
                if (collision.gameObject.GetComponent<PlayerController>().gotGoldSword)
                {
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
                }

                if (!collision.gameObject.GetComponent<PlayerController>().gotGoldSword)
                {
                    audioManager.Sword();
                    collision.gameObject.GetComponent<PlayerController>().GotSilverSword();
                    PickUpKey(collision);
                }
            }

            if (goldSword)
            {
                if (collision.gameObject.GetComponent<PlayerController>().gotSilverSword)
                {
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<CircleCollider2D>());
                }

                if (!collision.gameObject.GetComponent<PlayerController>().gotSilverSword)
                {
                    audioManager.Sword();
                    collision.gameObject.GetComponent<PlayerController>().GotGoldSword();
                    PickUpKey(collision);
                }
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (!bossKey)
                {
                    collision.gameObject.GetComponent<PlayerController>().GotKey();
                    PickUpKey(collision);
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

        if (silverSword && transform.rotation.y >= 0f || goldSword && transform.rotation.y >= 0f)
        {
            transform.position = new Vector2(collision.transform.position.x + 0.25f, collision.transform.position.y + 0.05f);
            StartCoroutine(ParryControl());
        }

        if (silverSword && transform.rotation.y < 0f || goldSword && transform.rotation.y < 0f)
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
        if (silverSword || goldSword)
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
            if (!bossKey && !silverSword && !goldSword)
            {
                promptText.SetActive(false);
            }            
        }
    }  
}

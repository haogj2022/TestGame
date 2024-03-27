using System.Collections;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private bool bossDoor;
    [SerializeField] private bool bossArea;
    [SerializeField] private CameraShake cameraShake;
    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.1f;

    private Animator anim;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (bossArea && anim.GetBool("Open"))
            {
                anim.SetBool("Open", false);
                cameraShake.Shake(camShakeAmt, camShakeLength);
            }
        }
    }

    public void BossAreaCleared()
    {
        anim.SetBool("Open", true);
        cameraShake.Shake(camShakeAmt, camShakeLength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().gotKey)
            {
                if (!bossDoor)
                {
                    hint.SetActive(false);
                }
                
                collision.gameObject.GetComponent<PlayerController>().DropKey();

                if (collision.gameObject.GetComponentInChildren<Key>() != null)
                {
                    collision.gameObject.GetComponentInChildren<Key>().DropKey();
                }
                
                anim.SetBool("Open", true);

                if (bossDoor)
                {
                    StartCoroutine(BossDoorOpen());
                }
            }
            else
            {
                if (!bossDoor)
                {
                    hint.transform.position = transform.position;
                    hint.SetActive(true);
                }                                  
            }
        }
    }

    IEnumerator BossDoorOpen()
    {
        yield return new WaitForSeconds(1f);
        cameraShake.Shake(camShakeAmt, camShakeLength);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!bossDoor)
            {
                hint.SetActive(false);
            }            
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

using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 playerRespawn;
    [SerializeField] private Animator anim;
    private PlayerController player;
    private bool canAttack;
    private Animator playerAnim;
    private PatrolCoroutines patrol;

    private void Start()
    {
        player = GameManager.Instance.playerController;
        playerAnim = player.gameObject.GetComponent<Animator>();
        patrol = GetComponentInParent<PatrolCoroutines>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (patrol != null)
            {
                StartCoroutine(StopPatrol());
            }
            
            anim.SetBool("Attack", true);
            canAttack = true;
        }
    }

    IEnumerator StopPatrol()
    {
        patrol._speed = 0f;
        yield return new WaitForSeconds(1.25f);
        patrol._speed = 2f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetBool("Attack", false);
            canAttack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
            StartCoroutine(RespawnCooldown());            
        }  
    }

    IEnumerator RespawnCooldown()
    {
        playerAnim.SetBool("Death", true);
        player.gameObject.SetActive(false);        
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("Death", false);                  
        player.ResetPosition(playerRespawn);
        player.gameObject.SetActive(true);
    }
}

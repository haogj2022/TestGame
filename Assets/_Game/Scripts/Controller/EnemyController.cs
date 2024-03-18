using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem respawnEffect;
    public Vector3 playerRespawn;
    [SerializeField] private Animator anim;
    [SerializeField] private bool keepMoving;
    [SerializeField] private float patrolDelay = 1f;
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
            if (patrol != null && !keepMoving)
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
        yield return new WaitForSeconds(patrolDelay);
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
            if (player.gotKey)
            {
                player.DropKey();

                if (player.gameObject.GetComponentInChildren<Key>() != null)
                {
                    player.gameObject.GetComponentInChildren<Key>().DropKey();
                }
            }

            player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
            player.GetComponent<Rigidbody2DSwim>().Dead();
            StartCoroutine(RespawnCooldown());            
        }  
    }

    IEnumerator RespawnCooldown()
    {
        playerAnim.SetBool("Death", true);
        deathEffect.transform.position = player.transform.position;
        player.gameObject.SetActive(false);
        deathEffect.Play();
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("Death", false);                  
        player.ResetPosition(playerRespawn);
        Camera.main.transform.position = new Vector3(playerRespawn.x, playerRespawn.y, -10f);
        respawnEffect.transform.position = playerRespawn;
        respawnEffect.Play();
        yield return new WaitForSeconds(1f);
        player.gameObject.SetActive(true);
        player.GetComponent<Rigidbody2DHorizontalMove>().Flip(true);
    }
}

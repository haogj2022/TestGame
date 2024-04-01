using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 playerRespawn;
    [SerializeField] private Animator anim;
    [SerializeField] private bool keepMoving;
    [SerializeField] private float patrolDelay = 1f;
    public bool isBoss;
    private AudioManager audioManager;
    private PlayerController player;
    [HideInInspector] public bool canAttack;
    private PatrolCoroutines patrol;
    [HideInInspector] public bool isAttacking;
    private GameObject boss;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("MainAudio").GetComponent<AudioManager>();
        player = GameManager.Instance.playerController;
        patrol = GetComponentInParent<PatrolCoroutines>();
        boss = GameObject.FindGameObjectWithTag("MainBoss");
    }

    private void Update()
    {
        if (!isBoss)
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), boss.GetComponent<BoxCollider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canAttack = true;

            if (patrol != null && !keepMoving)
            {
                StartCoroutine(StopPatrol());
            }

            anim.SetBool("Attack", true);
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
            audioManager.Dead();
            gameObject.SetActive(false);
        }
    }

    public void AttackReady()
    {
        isAttacking = true;
        StartCoroutine(AimAttack());
    }

    IEnumerator AimAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    public void Dead()
    {
        audioManager.Dead();
    }

    public void Attack()
    {
        if (!player.isVulnerable || player.gotSword && player.canParry)
        {
            anim.SetBool("Attack", false);
            canAttack = false;
        }

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

            if (player.gotSword)
            {
                player.DropSword();

                if (player.gameObject.GetComponentInChildren<Key>() != null)
                {
                    player.gameObject.GetComponentInChildren<Key>().DropSword();
                }
            }

            player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
            player.GetComponent<Rigidbody2DSwim>().Dead();
            GameManager.Instance.RespawnPlayer(playerRespawn);           
        }  
    }   
}

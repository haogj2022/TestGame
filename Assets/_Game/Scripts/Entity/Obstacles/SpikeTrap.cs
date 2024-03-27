using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Vector3 playerRespawn;   
    public Vector3 spikeRespawn;
    public Transform groundCheck;
    public BoxCollider2D hitBox;
    public float floorHeight = 1.25f;
    public ContactFilter2D filter;

    private float velocity;    
    private Collider2D[] results = new Collider2D[1];
    private bool canFall;
    private PlayerController player;
    private float gravity = -9.81f;
    private float gravityScale = 0.9f;
    private GameObject boss;
    private GameObject enemyDrop;

    private void Start()
    {
        player = GameManager.Instance.playerController;
        boss = GameObject.FindGameObjectWithTag("MainBoss");
        enemyDrop = GameObject.FindGameObjectWithTag("EnemyDrop");
    }

    public void FallingSpike()
    {
        StartCoroutine(Fall());        
    }

    private void Update()
    {
        if (canFall)
        {
            velocity += gravity * gravityScale * Time.deltaTime;
            transform.Translate(new Vector2(0f, velocity) * Time.deltaTime);

            if (groundCheck == null) return;
            if (hitBox == null) return;

            Physics2D.IgnoreCollision(hitBox, boss.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(hitBox, enemyDrop.GetComponent<BoxCollider2D>());

            if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0f, filter, results) > 0f && velocity < 0f)
            {
                velocity = 0f;
                hitBox.isTrigger = true;
                Vector2 surface = Physics2D.ClosestPoint(transform.position, results[0]) + Vector2.up * floorHeight;
                transform.position = new Vector2(transform.position.x, surface.y);               
            }
        }
        else
        {
            transform.position = spikeRespawn;
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(2f);
        float random = Random.Range(0f, 1f);

        if (random > 0.5f)
        {
            canFall = true;
            StartCoroutine(RespawnSpike());
        }
        else
        {
            StartCoroutine(DelayedFall());
        }
    }

    IEnumerator DelayedFall()
    {
        yield return new WaitForSeconds(1f);
        canFall = true;
        StartCoroutine(RespawnSpike());
    }

    IEnumerator RespawnSpike()
    {
        yield return new WaitForSeconds(2f);
        transform.position = spikeRespawn;
        canFall = false;
        hitBox.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.isVulnerable)
            {
                if (player.gotKey)
                {
                    player.DropKey();
                    player.gameObject.GetComponentInChildren<Key>().DropKey();
                }

                if (player.gotSword)
                {
                    player.DropSword();
                    player.gameObject.GetComponentInChildren<Key>().DropSword();
                }
                
                player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
                GameManager.Instance.RespawnPlayer(playerRespawn);
            }

            if (!player.isVulnerable)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
            }
        }
    }
}

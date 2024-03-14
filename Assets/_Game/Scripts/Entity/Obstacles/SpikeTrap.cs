using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem respawnEffect;
    public Vector3 playerRespawn;

    private PlayerController player;
    private Animator playerAnim;

    private void Start()
    {
        player = GameManager.Instance.playerController;
        playerAnim = player.gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().gotKey)
            {
                collision.gameObject.GetComponent<PlayerController>().DropKey();
                collision.gameObject.GetComponentInChildren<Key>().DropKey();
            }

            player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
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

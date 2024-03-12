using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
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

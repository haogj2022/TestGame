using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 playerRespawn;
    [SerializeField] private Animator anim;
    private PlayerController player;
    private bool canAttack;

    private void Start()
    {
        player = GameManager.Instance.playerController;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("Attack", true);
            canAttack = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetBool("Attack", false);
            canAttack = false;
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            player.ResetPosition(playerRespawn);
        }  
    }
}

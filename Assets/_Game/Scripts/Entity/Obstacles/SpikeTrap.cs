using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Vector3 playerRespawn;

    private PlayerController player;

    private void Start()
    {
        player = GameManager.Instance.playerController;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.ResetPosition(playerRespawn);
        }
    }
}

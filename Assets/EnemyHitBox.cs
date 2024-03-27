using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public Vector3 respawnLocation;
    public ParticleSystem skeletonDeathEffect;
    public GameObject swordDrop;
    private CameraController cameraController;
    private PlayerController player;

    private void Awake()
    {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallingSpike" || collision.gameObject.tag == "Player" && player.gotSword && !player.canParry)
        {
            Dead();
        }
    }

    public void Dead()
    {
        cameraController.RespawnSkeleton();
        skeletonDeathEffect.transform.position = gameObject.transform.position;
        gameObject.SetActive(false);

        if (!player.gotSword)
        {
            swordDrop.transform.position = gameObject.transform.position;
        }

        skeletonDeathEffect.Play();
    }
}

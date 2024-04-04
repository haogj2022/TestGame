using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public Vector3 respawnLocation;
    public ParticleSystem skeletonDeathEffect;
    public GameObject swordDrop;
    public GameObject swordDropGold;
    private CameraController cameraController;
    private PlayerController player;
    public AudioManager audioManager;
    private Boss boss;

    private void Awake()
    {
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        boss = GameObject.FindGameObjectWithTag("MainBoss").GetComponent<Boss>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallingSpike" || collision.gameObject.tag == "Player" && player.gotSilverSword && !player.canParry || 
            collision.gameObject.tag == "Player" && player.gotGoldSword && !player.canParry)
        {
            Dead();
        }
    }

    public void Dead()
    {
        audioManager.Dead();
        cameraController.RespawnSkeleton();
        skeletonDeathEffect.transform.position = gameObject.transform.position;
        gameObject.SetActive(false);

        if (!player.gotSilverSword && !player.gotGoldSword)
        {
            if (boss.bossHealthBar.value <= 0.75f)
            {
                float random = Random.Range(0f, 1f);

                if (random > 0.4f)
                {
                    swordDropGold.transform.position = gameObject.transform.position;
                }
                else
                {
                    swordDrop.transform.position = gameObject.transform.position;
                }
            }
            else
            {
                swordDrop.transform.position = gameObject.transform.position;
            }                 
        }

        skeletonDeathEffect.Play();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Transform player;
    public Slider bossHealthBar;
    public GameObject horizontalMagicSlash;
    public GameObject verticalMagicSlash;
    public LockedDoor bossDoor;
    public EnemyHitBox skeletonWarrior;
    public AudioManager audioManager;

    private bool isFacingRight = true;
    private SpriteRenderer hitColor;
    private Animator anim;
    private ParticleSystem dust;
    private CameraController cameraController;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        hitColor = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        dust = GetComponentInChildren<ParticleSystem>();
        cameraController = Camera.main.GetComponent<CameraController>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (bossHealthBar.value <= 0.75f)
        {
            anim.SetBool("Phase 2", true);

            if (bossHealthBar.value <= 0.5f)
            {
                anim.SetBool("Phase 3", true);

                if (bossHealthBar.value <= 0.25f)
                {
                    anim.SetBool("Phase 4", true);                   
                }
            }
        }

        if (bossHealthBar.value < 0.05f)
        {
            anim.SetBool("Defeat", true);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.transform.position.x && isFacingRight)
        {
            CreateDust();
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = false;
        }
        else if (transform.position.x < player.transform.position.x && !isFacingRight)
        {
            CreateDust();
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = true;
        }
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.GetComponent<PlayerController>().gotSword && !player.GetComponent<PlayerController>().canParry)
        {
            audioManager.Dead();
            player.GetComponent<PlayerController>().DropSword();
            player.gameObject.GetComponentInChildren<Key>().DropSword();
            bossHealthBar.value -= 0.05f;
            StartCoroutine(GotHit());

            if (bossHealthBar.value < 0.05f)
            {
                audioManager.Background();
                anim.SetBool("Defeat", true);
                Debug.Log("Boss Defeated");
                StartCoroutine(BossFightCompleted());
            }
        }
    }

    IEnumerator BossFightCompleted()
    {
        cameraController.bossUI.SetActive(false);
        boxCollider2D.isTrigger = true;
        player.GetComponent<PlayerController>().isVulnerable = false;
        GameManager.Instance.EnableShield();
        bossDoor.BossAreaCleared();
        skeletonWarrior.Dead();
        cameraController.respawnSkeleton = false;
        skeletonWarrior.Dead();
        yield return new WaitForSeconds(5f);
        cameraController.bossRoom = false;
        player.GetComponent<PlayerController>().isVulnerable = true;
        GameManager.Instance.DisableShield();
    }

    IEnumerator GotHit()
    {
        player.GetComponent<PlayerController>().isVulnerable = false;
        GameManager.Instance.EnableShield();
        hitColor.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        hitColor.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<PlayerController>().isVulnerable = true;
        GameManager.Instance.DisableShield();
    }

    public void HorizontalMagicSlash()
    {
        StartCoroutine(HorizontalMagicForward());
    }

    public void VerticalMagicSlash()
    {
        StartCoroutine(VerticalMagicForward());
    }

    IEnumerator HorizontalMagicForward()
    {
        horizontalMagicSlash.transform.position = transform.position;
        horizontalMagicSlash.SetActive(true);
        horizontalMagicSlash.GetComponent<TransformForward>().LocatePlayer();
        yield return new WaitForSeconds(1f);
        horizontalMagicSlash.SetActive(false);
    }

    IEnumerator VerticalMagicForward()
    {
        verticalMagicSlash.transform.position = transform.position;
        verticalMagicSlash.SetActive(true);
        verticalMagicSlash.GetComponent<TransformForward>().LocatePlayer();
        yield return new WaitForSeconds(1f);
        verticalMagicSlash.SetActive(false);
    }
}

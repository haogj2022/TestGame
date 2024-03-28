using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public Vector3 bossRoomLocation;
    public Vector3 bossLandingLocation;
    public GameObject bossUI;
    public GameObject skeletonWarrior;
    public Vector3 respawnLocation;
    private GameObject player;
    [HideInInspector] public bool bossRoom;
    [HideInInspector] public bool respawnSkeleton = true;

    private Light2D cameraLight;

    private bool bossPreview;

    private Rigidbody2D playerRb2D;

    private Animator bossAnim;

    private void Awake()
    {
        cameraLight = GameObject.FindGameObjectWithTag("CameraLight").GetComponent<Light2D>();
        playerRb2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        bossAnim = GameObject.FindGameObjectWithTag("MainBoss").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerController.gameObject.transform.position.y < 25.25f && bossRoom)
        {
            GameManager.Instance.playerController.gameObject.transform.position = new Vector2(GameManager.Instance.playerController.gameObject.transform.position.x, 25.25f);
        }

        if (bossPreview)
        {
            player = GameObject.FindGameObjectWithTag("MainBoss");
            player.transform.position = Vector2.MoveTowards(player.transform.position, bossLandingLocation, Time.deltaTime * 2f);

            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 20f);

            playerRb2D.bodyType = RigidbodyType2D.Static;
        }

        playerRb2D.bodyType = RigidbodyType2D.Dynamic;

        if (bossRoom && !bossPreview)
        {
            CameraLocked();
        }

        if (!bossRoom)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);

            if (!player.GetComponent<Animator>().GetBool("Tunnel"))
            {
                Camera.main.orthographicSize = 4f;
                cameraLight.pointLightOuterRadius = 10f;
            }
        }
    }

    public void RespawnSkeleton()
    {
        if (respawnSkeleton)
        {
            StartCoroutine(Respawn());
        }        
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        skeletonWarrior.transform.position = respawnLocation;
        skeletonWarrior.SetActive(true);
        skeletonWarrior.GetComponent<PatrolCoroutines>().Continue();
    }

    public void ShowBoss()
    {
        StartCoroutine(BossPreview());
    }

    public void CameraLocked()
    {
        transform.position = new Vector3(bossRoomLocation.x, bossRoomLocation.y, -10f);
        Camera.main.orthographicSize = 8f;
        cameraLight.pointLightOuterRadius = 20f;
    }

    IEnumerator BossPreview()
    {
        bossRoom = true;
        bossPreview = true;
        bossAnim.SetTrigger("Intro");
        yield return new WaitForSeconds(3f);
        bossPreview = false;
        player = null;
        bossAnim.ResetTrigger("Intro");
        bossUI.SetActive(true);
        RespawnSkeleton();
        CameraLocked();
    }
}
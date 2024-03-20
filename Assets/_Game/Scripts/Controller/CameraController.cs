using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public Vector3 bossRoomLocation;
    private GameObject player;
    [HideInInspector] public bool bossRoom;

    private Light2D cameraLight;

    private bool bossPreview;

    private Rigidbody2D playerRb2D;

    private void Awake()
    {
        cameraLight = GameObject.FindGameObjectWithTag("CameraLight").GetComponent<Light2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossPreview)
        {
            player = GameObject.FindGameObjectWithTag("MainBoss");
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 10f);

            playerRb2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            playerRb2D.bodyType = RigidbodyType2D.Static;
        }

        playerRb2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerRb2D.bodyType = RigidbodyType2D.Dynamic;

        if (!bossRoom)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            Camera.main.orthographicSize = 4f;
            cameraLight.pointLightOuterRadius = 10f;
        }
    }

    public void ShowBoss()
    {
        StartCoroutine(BossPreview());
    }

    IEnumerator BossPreview()
    {
        yield return new WaitForSeconds(0.5f);
        bossRoom = true;
        bossPreview = true;       
        yield return new WaitForSeconds(3f);
        bossPreview = false;
        player = null;
        transform.position = new Vector3(bossRoomLocation.x, bossRoomLocation.y, -10f);
        Camera.main.orthographicSize = 7f;
        cameraLight.pointLightOuterRadius = 20f;        
    }
}
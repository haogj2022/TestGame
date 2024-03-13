using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Tunnel : MonoBehaviour
{
    [SerializeField] private bool isEntrance;
    [SerializeField] private GameObject promptText;

    private float horizontalFloat;
    private float verticalFloat;
    private float moveSpeed = 3f;

    private GameObject player;
    private Rigidbody2D rb2D;
    private BoxCollider2D boxCollider2D;
    private Animator anim;
    private Rigidbody2DFly rb2DFly;
    private PlayerController playerController;

    private Light2D cameraLight;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb2D = player.GetComponent<Rigidbody2D>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        anim = player.GetComponent<Animator>();
        rb2DFly = player.GetComponent<Rigidbody2DFly>();
        playerController = player.GetComponent<PlayerController>();

        cameraLight = GameObject.FindGameObjectWithTag("CameraLight").GetComponent<Light2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {           
            if (isEntrance)
            {
                promptText.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
                promptText.SetActive(true);

                if (Input.GetKey(KeyCode.E))
                {
                    rb2DFly.StartCoroutine(rb2DFly.TransformEffect());
                    playerController.ShowControl();
                    promptText.SetActive(false);
                    player.transform.position = transform.position;
                    EnterTunnel();
                }
            }           
        }        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isEntrance)
            {
                promptText.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isEntrance)
            {
                EnterTunnel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isEntrance)
            {
                ExitTunnel();
            }   
        }
    }

    private void EnterTunnel()
    {
        anim.SetBool("Tunnel", true);
        boxCollider2D.enabled = false;
        rb2D.gravityScale = 0f;
        Camera.main.orthographicSize = 2f;
        cameraLight.pointLightOuterRadius = 5f;
    }

    private void ExitTunnel()
    {
        anim.SetBool("Tunnel", false);
        boxCollider2D.enabled = true;
        rb2D.gravityScale = 1f;
        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Camera.main.orthographicSize = 4f;
        cameraLight.pointLightOuterRadius = 10f;
    }

    private void Update()
    {
        horizontalFloat = Input.GetAxisRaw("Horizontal");
        verticalFloat = Input.GetAxisRaw("Vertical");

        if (anim.GetBool("Tunnel"))
        {
            anim.SetFloat("Velocity", 0f);
            rb2D.velocity = new Vector2(horizontalFloat * moveSpeed, verticalFloat * moveSpeed);

            TunnelDIrection();
        }
    }  

    private void TunnelDIrection()
    {
        TunnelHorizontal();
        TunnelVertical();
    }

    private void TunnelHorizontal()
    {

        if (horizontalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }

        if (horizontalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }

    private void TunnelVertical()
    {
        if (verticalFloat > 0f)
        {
            anim.SetFloat("Velocity", 1f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        if (verticalFloat < 0f)
        {
            anim.SetFloat("Velocity", 1f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}

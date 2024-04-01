using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject abilityBar;
    [SerializeField] Animator tunnelControl;
    [SerializeField] Animator gemCollected;
    [SerializeField] Animator keyCollected;
    [HideInInspector] public bool gotKey;
    [HideInInspector] public bool gotSword;
    [HideInInspector] public bool isVulnerable = true;
    [HideInInspector] public bool canParry;
    public AudioManager audioManager;
    private Animator anim;
    private Key sword;
    private CameraController cameraController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    private void Update()
    {
        if (anim.GetBool("Fly"))
        {
            ActivateAbility();
        }

        if (gotSword)
        {
            sword = GetComponentInChildren<Key>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Parry());
            }           
        }        
    }   

    IEnumerator Parry()
    {
        canParry = true;
        sword.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f);
        yield return new WaitForSeconds(1f);
        sword.transform.rotation = transform.rotation;
        canParry = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BossRoom")
        {
            cameraController.ShowBoss();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp" || collision.gameObject.tag == "Item" && Input.GetKey(KeyCode.E))
        {
            ActivateAbility();
        }

        if (collision.gameObject.tag == "Gem")
        {
            audioManager.Gem();
            collision.gameObject.SetActive(false);
            StartCoroutine(GemCollected());
        }

        if (collision.gameObject.tag == "BossKey")
        {
            audioManager.Gem();
            StartCoroutine(KeyCollected());
        }

        if (collision.gameObject.tag == "Door")
        {
            CancelAbility();
        }
    }

    IEnumerator GemCollected()
    {
        gemCollected.SetBool("Collected", true);
        yield return new WaitForSeconds(2f);
        gemCollected.SetBool("Collected", false);
    }

    IEnumerator KeyCollected()
    {
        keyCollected.SetBool("Collected", true);
        yield return new WaitForSeconds(2f);
        keyCollected.SetBool("Collected", false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp" || collision.gameObject.tag == "Death")
        {
            CancelAbility();
        }
    }

    public void GotKey()
    {
        gotKey = true;
    }

    public void DropKey()
    {
        gotKey = false;
    }

    public void GotSword()
    {
        gotSword = true;
    }

    public void DropSword()
    {
        gotSword = false;
    }

    public void ActivateAbility()
    {
        abilityBar.SetActive(true);
    }

    public void CancelAbility()
    {
        abilityBar.SetActive(false);
        abilityBar.GetComponent<Slider>().value = abilityBar.GetComponent<Slider>().maxValue;
    }

    public void ShowControl()
    {
        StartCoroutine(ShowTunnelControl());        
    }

    IEnumerator ShowTunnelControl()
    {
        tunnelControl.SetBool("Left", true);
        yield return new WaitForSeconds(3f);
        tunnelControl.SetBool("Left", false);
    }

    public void ResetPosition(Vector2 playerRespawn)
    {
        CancelAbility();
        canParry = false;
        transform.position = playerRespawn;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}

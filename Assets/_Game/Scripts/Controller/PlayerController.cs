using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject abilityBar;
    [SerializeField] Animator tunnelControl;
    [HideInInspector] public bool gotKey;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (anim.GetBool("Fly"))
        {
            ActivateAbility();
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
            Debug.Log("Gem collected!");
            collision.gameObject.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LedgeUp")
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
        transform.position = playerRespawn;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}

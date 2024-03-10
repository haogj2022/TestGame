using System.Collections;
using UnityEngine;

public class LightPlant : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().gotKey)
            {
                anim.SetBool("Off", true);
                StartCoroutine(GrowSeed());
            }
        }
    }

    IEnumerator GrowSeed()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("Off", false);
        StopAllCoroutines();
    }
}

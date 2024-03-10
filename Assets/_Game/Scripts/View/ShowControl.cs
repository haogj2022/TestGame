using System.Collections;
using UnityEngine;

public class ShowControl : MonoBehaviour
{
    [SerializeField] Animator showControl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            showControl.SetBool("Left", true);
            StartCoroutine(HideControl());
        }
    }

    IEnumerator HideControl()
    {
        yield return new WaitForSeconds(3f);
        showControl.SetBool("Left", false);
    }
}

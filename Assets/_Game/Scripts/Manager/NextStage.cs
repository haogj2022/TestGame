using UnityEngine;

public class NextStage : MonoBehaviour
{
    public Vector3 nextLocation;
    [SerializeField] private GameObject promptText;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            promptText.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
            promptText.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                collision.transform.position = nextLocation;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            promptText.SetActive(false);
        }
    }
}

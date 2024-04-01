using UnityEngine;

public class TransformForward : MonoBehaviour
{
    public float speed = 5f;

    public Vector3 playerRespawn;

    private PlayerController player;

    public void LocatePlayer()
    {
        player = GameManager.Instance.playerController;

        if (player != null)
        {
            Vector3 look = transform.InverseTransformPoint(player.transform.position);
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;

            transform.Rotate(0f, 0f, angle);
        }
    }

    public void Parry()
    {
        player = GameManager.Instance.playerController;

        if (player != null)
        {
            Vector3 look = transform.InverseTransformPoint(player.transform.position);
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;

            transform.Rotate(0f, 0f, angle + 180f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.isVulnerable && !player.gotSword || player.gotSword && !player.canParry)
            {
                if (player.gotKey)
                {
                    player.DropKey();
                    player.gameObject.GetComponentInChildren<Key>().DropKey();
                }

                if (player.gotSword)
                {
                    player.DropSword();
                    player.gameObject.GetComponentInChildren<Key>().DropSword();
                }

                player.GetComponent<Rigidbody2DHorizontalMove>().Dead();
                GameManager.Instance.RespawnPlayer(playerRespawn);
            }

            if (player.gotSword && player.canParry)
            {
                Parry();
            }

            if (!player.isVulnerable)
            {
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
            }
        }
    }
}

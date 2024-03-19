using UnityEngine;

public class LookAt : MonoBehaviour
{
    private GameObject player;
    private EnemyController mage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mage.canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mage.canAttack = false;
        }
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mage = GetComponentInParent<EnemyController>();

        if (player != null && !mage.isAttacking)
        {
            Vector3 look = transform.InverseTransformPoint(player.transform.position);
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;

            transform.Rotate(0f, 0f, angle);
        }
    }
}

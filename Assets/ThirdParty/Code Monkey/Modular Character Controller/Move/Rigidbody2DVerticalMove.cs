using UnityEngine;

public class Rigidbody2DVerticalMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private float verticalFloat;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        verticalFloat = Input.GetAxis("Vertical");
        rb2D.velocity = new Vector2(rb2D.velocity.x, verticalFloat * moveSpeed);
    }
}

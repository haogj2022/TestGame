using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    public float speed = 3f;
    public float attackRange = 5f;
    public float gravityScale = 7f;
    public float floorHeight = 1.25f;
    public ContactFilter2D filter;

    private Transform player;
    private Rigidbody2D rb2D;
    private Boss boss;
    private float velocity;
    private Transform groundCheck;
    private Collider2D[] results = new Collider2D[1];
    private bool isGrounded;
    private float gravity = -9.81f;
    private float offset = 1f;
    private Vector2 target;
    private Vector2 newPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2D = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        groundCheck = GameObject.FindGameObjectWithTag("BossGroundCheck").GetComponent<Transform>();
        isGrounded = true;
        player = boss.GetComponent<Boss>().player;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        velocity += gravity * gravityScale * Time.deltaTime;

        if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0f, filter, results) > 0f && velocity < 0f)
        {
            velocity = 0f;
            Vector2 surface = Physics2D.ClosestPoint(animator.transform.position, results[0]) + Vector2.up * floorHeight;
            animator.transform.position = new Vector2(animator.transform.position.x, surface.y);
        }

        if (animator.transform.position.y < 26.25f)
        {
            animator.transform.position = new Vector2(animator.transform.position.x, 26.25f);
        }

        if (animator.transform.position.y > 35.25f)
        {
            animator.transform.position = new Vector2(animator.transform.position.x, 35.25f);
        }

        if (animator.transform.position.x > 247.5f)
        {
            animator.transform.position = new Vector2(247.5f, animator.transform.position.y);
        }

        if (animator.transform.position.x < 231.5f)
        {
            animator.transform.position = new Vector2(231.5f, animator.transform.position.y);
        }

        if (isGrounded)
        {
            velocity = Mathf.Sqrt(-2f * (gravity * gravityScale));
            isGrounded = false;
        }

        animator.transform.Translate(new Vector2(0f, velocity) * Time.deltaTime);

        boss.LookAtPlayer();        

        if (player.position.x >= animator.transform.position.x)
        {
            target = new Vector2(player.position.x - offset, rb2D.position.y);
        }
        else
        {
            target = new Vector2(player.position.x + offset, rb2D.position.y);
        }

        newPos = Vector2.MoveTowards(rb2D.position, target, speed * Time.fixedDeltaTime);
        rb2D.MovePosition(newPos);

        if (Vector3.Distance(target, rb2D.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}

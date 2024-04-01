using UnityEngine;

public class BossGroundSmash : StateMachineBehaviour
{
    public float jumpHeight = 7f;
    public float attackRange = 5f;
    public float gravityScale = 7f;
    public float floorHeight = 1.25f;    
    public ContactFilter2D filter;

    private float velocity;
    private Transform groundCheck;
    private bool isGrounded;
    private Collider2D[] results = new Collider2D[1];
    private float newPos;
    private Transform player;
    private float distance;
    private float gravity = -9.81f;
    private float offset = 1f;
    private CameraShake cameraShake;
    private float camShakeAmt = 0.01f;
    private float camShakeLength = 2f;
    private GameObject[] spikeTrap;
    private Boss boss;
    private CameraController cameraController;
    private ParticleSystem dust;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dust = animator.GetComponentInChildren<ParticleSystem>();

        cameraController = Camera.main.GetComponent<CameraController>();
        boss = animator.GetComponent<Boss>();
        player = boss.GetComponent<Boss>().player;

        if (player.position.x >= animator.transform.position.x)
        {
            Vector2 target = new Vector2(player.position.x - offset, animator.transform.position.y);
            distance = Vector3.Distance(target, animator.transform.position);
        }
        else
        {
            Vector2 target = new Vector2(player.position.x + offset, animator.transform.position.y);
            distance = Vector3.Distance(target, animator.transform.position);
        }

        cameraShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();

        spikeTrap = GameObject.FindGameObjectsWithTag("FallingSpike");
        foreach (GameObject spike in spikeTrap)
        {
            spike.GetComponent<SpikeTrap>().FallingSpike();
        }

        groundCheck = GameObject.FindGameObjectWithTag("BossGroundCheck").GetComponent<Transform>();       
        isGrounded = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        velocity += gravity * gravityScale * Time.deltaTime;

        if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0f, filter, results) > 0f && velocity < 0f)
        {
            velocity = 0f;
            newPos = 0f;
            Vector2 surface = Physics2D.ClosestPoint(animator.transform.position, results[0]) + Vector2.up * floorHeight;
            animator.transform.position = new Vector2(animator.transform.position.x, surface.y);
            cameraShake.Shake(camShakeAmt, camShakeLength);
            cameraController.skeletonWarrior.GetComponent<PatrolCoroutines>().Interupt();
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
            velocity = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale));
            CreateDust();
            isGrounded = false;

            newPos = distance;
        }

        animator.transform.Translate(new Vector2(newPos, velocity) * Time.deltaTime);     
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void CreateDust()
    {
        dust.Play();
    }
}

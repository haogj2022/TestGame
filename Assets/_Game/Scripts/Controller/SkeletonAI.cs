using System.Collections;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] float reactionTime;
    [SerializeField] float raycastRange;
    [SerializeField] GameObject exclamationMark;
    [SerializeField] GameObject guideIcon;
    [SerializeField] Transform eyeLocation;
    EnemyController controller;
    PatrolCoroutines patrol;
    Animator animator;
    bool chasing;
    void Start()
    {
        patrol = GetComponent<PatrolCoroutines>();
        animator = GetComponent<Animator>();
        controller = GetComponent<EnemyController>();
    }

    void FixedUpdate()
    {
        if (chasing) return;
        RaycastHit2D hit = Physics2D.BoxCast(eyeLocation.position, new Vector2(1, 1), 0, GetDirection(), raycastRange, 1 << 6);
        Debug.DrawRay(eyeLocation.position, GetDirection() * raycastRange, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Detected Player, Attacking...");
            StartCoroutine(Attack(hit.transform.position));
        }
    }

    IEnumerator Attack(Vector3 targetPosition)
    {
        chasing = true;
        patrol.Interupt();
        guideIcon.SetActive(false);
        exclamationMark.SetActive(true);
        yield return new WaitForSeconds(reactionTime);
        exclamationMark.SetActive(false);
        guideIcon.SetActive(true);
        animator.SetBool("Idle", false);
        Vector3 location = new(targetPosition.x, transform.position.y,  transform.position.z);
        while (Vector3.Distance(transform.position, location) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, location, speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Done Chasing");
        animator.SetBool("Attack", true);
        animator.speed = 5f;
        controller.Attack();
        animator.speed = 1f;
        yield return new WaitForSeconds(reactionTime);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", true);
        patrol.Continue();
        chasing = false;
    }

    Vector2 GetDirection()
    {
        if (transform.rotation.y != 0)
            return Vector2.left;
        else
            return Vector2.right;
    }
}

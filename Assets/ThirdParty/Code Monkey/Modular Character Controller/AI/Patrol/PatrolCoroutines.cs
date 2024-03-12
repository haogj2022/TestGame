using System.Collections;
using UnityEngine;

public class PatrolCoroutines : MonoBehaviour
{
    public Transform[] waypoints;
    private int _currentWaypointIndex = 0;
    [HideInInspector] public float _speed = 2f;

    private float _waitTime = 1f; // in seconds

    private Coroutine _prevCoroutine;

    private bool isFacingRight = true;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _prevCoroutine = StartCoroutine(_MovingToNextWaypoint());
    }

    private IEnumerator _MovingToNextWaypoint()
    {
        Transform wp = waypoints[_currentWaypointIndex];

        while (Vector3.Distance(transform.position, wp.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wp.position, _speed * Time.deltaTime);
            yield return null;
        }

        transform.position = wp.position;
        anim.SetBool("Idle", true);
        yield return new WaitForSeconds(_waitTime);
        Flip();
        anim.SetBool("Idle", false);
        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;

        StopCoroutine(_prevCoroutine);
        _prevCoroutine = StartCoroutine(_MovingToNextWaypoint());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(DetectCooldown());
        }
    }

    IEnumerator DetectCooldown()
    {
        yield return new WaitForSeconds(1f);
        StopAllCoroutines();
        Flip();
        anim.SetBool("Idle", false);
        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        Continue();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (!isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    public void Interupt()
    {
        StopCoroutine(_prevCoroutine);
    }

    public void Continue()
    {
        _prevCoroutine = StartCoroutine(_MovingToNextWaypoint());
    }
}

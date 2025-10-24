using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;

    [Header("Detection Settings")]
    public float sightRange = 10f;
    public float fieldOfView = 60f;
    public LayerMask obstacleMask;

    [Header("Chase Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("References")]
    public Animator animator;
    public string attackTrigger = "Attack";

    private NavMeshAgent agent;
    private Transform player;
    private int currentPoint = 0;
    private bool chasing = false;
    private bool gameOver = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = patrolSpeed;
        GoToNextPoint();
    }

    void Update()
    {
        if (gameOver) return;

        if (!chasing)
        {
            Patrol();
            DetectPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPoint].position;
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
    }

    void DetectPlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < fieldOfView / 2 && dirToPlayer.magnitude < sightRange)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer.normalized, dirToPlayer.magnitude, obstacleMask))
            {
                chasing = true;
                agent.speed = chaseSpeed;
            }
        }
    }

    void ChasePlayer()
    {
        agent.destination = player.position;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 1.8f)
        {
            StartCoroutine(AttackAndExit());
        }
    }

    System.Collections.IEnumerator AttackAndExit()
    {
        chasing = false;
        gameOver = true;
        agent.isStopped = true;

        if (animator)
            animator.SetTrigger(attackTrigger);

        yield return new WaitForSeconds(2.5f);

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

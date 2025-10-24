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
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentPoint = 0;
    private bool chasing = false;
    private float waitTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = patrolSpeed;
        GoToNextPoint();
    }

    void Update()
    {
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
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                GoToNextPoint();
                waitTimer = 0f;
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void DetectPlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < fieldOfView * 0.5f && dirToPlayer.magnitude <= sightRange)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer.normalized, dirToPlayer.magnitude, obstacleMask))
            {
                chasing = true;
                agent.speed = chaseSpeed;
                Debug.Log("Enemy spotted player!");
            }
        }
    }

    void ChasePlayer()
    {
        agent.destination = player.position;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 1.5f)
        {
            Debug.Log("Player caught!");
            chasing = false;
            agent.speed = patrolSpeed;
            GoToNextPoint();
        }
    }
}

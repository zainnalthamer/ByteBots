using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MarketNPCWander : MonoBehaviour
{
    [Header("Market Points")]
    public Transform[] boothPoints;

    [Header("Movement")]
    public float walkSpeed = 1.8f;
    public float rotationSpeed = 6f;

    [Header("Idle")]
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;

    NavMeshAgent agent;
    Animator animator;
    Transform currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
    }

    void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    IEnumerator WanderRoutine()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        while (true)
        {
            PickNewTarget();
            if (!currentTarget)
                yield break;

            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);
            animator.SetBool("Walk", true);

            while (agent.pathPending)
                yield return null;

            while (agent.remainingDistance > 0.2f)
                yield return null;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();

            animator.SetBool("Walk", false);

            yield return StartCoroutine(LookAtTarget());
            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        }
    }


    void PickNewTarget()
    {
        if (boothPoints.Length == 0) return;

        currentTarget = boothPoints[Random.Range(0, boothPoints.Length)];
    }

    IEnumerator LookAtTarget()
    {
        float t = 0f;
        Quaternion startRot = transform.rotation;
        Vector3 lookDir = (currentTarget.position - transform.position).normalized;
        lookDir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }
}

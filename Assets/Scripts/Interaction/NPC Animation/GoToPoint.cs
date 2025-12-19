using UnityEngine;
using UnityEngine.AI;

public class GoToPoint : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform targetPoint;

    bool isMoving = false;

    public void Go(Transform target)
    {
        if (!agent || !target) return;

        agent.isStopped = false;
        agent.SetDestination(target.position);

        isMoving = true;

        if (animator)
            animator.SetBool("Walk", true);
    }

    void Update()
    {
        if (!isMoving || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            agent.ResetPath();
            isMoving = false;

            if (animator)
                animator.SetBool("Walk", false);
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

public class LurkerEnterance : MonoBehaviour
{
    public Transform endPoint;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    public void StartEntrance()
    {
        agent.enabled = true;
        agent.SetDestination(endPoint.position);
    }
}

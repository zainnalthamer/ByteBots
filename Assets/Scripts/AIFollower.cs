using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    public Transform player;          
    public Vector3 followOffset = new Vector3(-1.2f, 0f, -1.0f); 
    public float stopDistance = 1.25f; 
    public float rotateSpeed = 10f;

    NavMeshAgent agent;
    Animator anim;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        if (!player || !agent) return;

        Vector3 target = player.TransformPoint(followOffset);

        float dist = Vector3.Distance(transform.position, target);

        if (dist > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        else
        {
            agent.isStopped = true; 
        }

        if (anim)
        {
            float speed = agent.velocity.magnitude;

            bool isWalking = speed > 0.1f;
            anim.SetBool("isWalking", isWalking);
        }

        Vector3 lookDir = player.forward;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }
}

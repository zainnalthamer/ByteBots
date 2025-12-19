using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BugAgent : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform roamCenter;
    [SerializeField] private float roamRadius = 8f;
    [SerializeField] private Vector2 idleWaitRange = new Vector2(0.5f, 2f);

    [Header("Anim")]
    [SerializeField] private Animator animator;
    [SerializeField] private string moveParam = "Speed";
    [SerializeField] private string dieTrigger = "Die";
    [SerializeField] private float deathDelay = 2.5f;

    private NavMeshAgent agent;
    private bool isDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!roamCenter) roamCenter = transform;
    }

    void OnEnable()
    {
        if (!isDead) StartCoroutine(RoamLoop());
    }

    System.Collections.IEnumerator RoamLoop()
    {
        var wait = new WaitForEndOfFrame();
        while (!isDead)
        {
            Vector3 target = RandomPoint(roamCenter.position, roamRadius);
            agent.SetDestination(target);

            // move until close to destination
            while (!isDead && agent.pathPending == false)// && agent.remainingDistance > agent.stoppingDistance)
            {
               // if (animator) animator.SetFloat(moveParam, agent.velocity.magnitude);
                yield return wait;
            }

            if (animator) animator.SetFloat(moveParam, 0f);
            // idle a bit
            float idle = Random.Range(idleWaitRange.x, idleWaitRange.y);
            float t = 0f;
            while (!isDead && t < idle) { t += Time.deltaTime; yield return wait; }
        }
    }

    Vector3 RandomPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 rand = center + Random.insideUnitSphere * radius;
            rand.y = center.y;
            if (NavMesh.SamplePosition(rand, out var hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }
        return center;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        agent.isStopped = true;
        if (animator) animator.SetTrigger(dieTrigger);
        Invoke(nameof(DisableSelf), deathDelay);
    }

    void DisableSelf() => gameObject.SetActive(false);
}

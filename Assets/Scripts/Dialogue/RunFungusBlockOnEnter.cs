using UnityEngine;
using Fungus;
using UnityEngine.AI;
using System.Collections;

public class RunFungusBlockOnEnter : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName;

    public NavMeshAgent bunnyAgent;
    public Animator bunnyAnimator;
    public Transform patch;
    public Transform exitPoint;

    private bool hasTriggered;
    private bool dialogueFinished;
    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        if (triggerCollider) triggerCollider.enabled = false;

        StartCoroutine(ApproachPatch());
    }

    private IEnumerator ApproachPatch()
    {
        bunnyAgent.isStopped = false;
        bunnyAnimator.Play("Run Forward In Place");
        bunnyAgent.SetDestination(patch.position);

        while (bunnyAgent.pathPending || bunnyAgent.remainingDistance > bunnyAgent.stoppingDistance)
            yield return null;

        bunnyAgent.isStopped = true;
        bunnyAgent.velocity = Vector3.zero;

        bunnyAnimator.Play("Idle");

        Vector3 dir = patch.position - bunnyAgent.transform.position;
        dir.y = 0f;
        bunnyAgent.transform.rotation = Quaternion.LookRotation(dir);

        yield return new WaitForSeconds(0.25f);

        bunnyAnimator.Play("Idle 3");

        if (flowchart && !string.IsNullOrEmpty(blockName))
            flowchart.ExecuteBlock(blockName);
    }

    public void OnDialogueFinished()
    {
        if (dialogueFinished) return;
        dialogueFinished = true;

        StopAllCoroutines();
        StartCoroutine(WalkAway());
    }

    private IEnumerator WalkAway()
    {
        bunnyAnimator.Play("Run Forward In Place");
        bunnyAgent.isStopped = false;
        bunnyAgent.SetDestination(exitPoint.position);

        while (bunnyAgent.pathPending || bunnyAgent.remainingDistance > bunnyAgent.stoppingDistance)
            yield return null;

        bunnyAgent.isStopped = true;
        bunnyAgent.velocity = Vector3.zero;

        bunnyAnimator.Play("Idle");
    }
}

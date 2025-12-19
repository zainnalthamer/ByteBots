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
    public Transform bunnyReturnPoint;

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

        Vector3 lookDir = patch.position - bunnyAgent.transform.position;
        lookDir.y = 0f;
        bunnyAgent.transform.rotation = Quaternion.LookRotation(lookDir);

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
        StartCoroutine(ReturnToPoint());
    }

    private IEnumerator ReturnToPoint()
    {
        bunnyAnimator.Play("Run Forward In Place");
        bunnyAgent.isStopped = false;
        bunnyAgent.SetDestination(bunnyReturnPoint.position);

        while (bunnyAgent.pathPending || bunnyAgent.remainingDistance > bunnyAgent.stoppingDistance)
            yield return null;

        bunnyAgent.isStopped = true;
        bunnyAgent.velocity = Vector3.zero;

        bunnyAnimator.Play("Idle");
    }
}

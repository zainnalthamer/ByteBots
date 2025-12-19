using UnityEngine;
using System.Collections;

public class SimpleWander : MonoBehaviour
{
    [Header("Wander Points")]
    public Transform[] wanderPoints;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float waitTimeAtPoint = 2f;

    private int currentIndex = 0;
    private bool canWander = false;

    void Update()
    {
        if (!canWander || wanderPoints.Length == 0) return;

        Transform target = wanderPoints[currentIndex];
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;

        if (direction.magnitude < 0.2f)
        {
            StartCoroutine(WaitAndMoveNext());
            canWander = false;
            return;
        }

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 5f
        );
    }

    IEnumerator WaitAndMoveNext()
    {
        yield return new WaitForSeconds(waitTimeAtPoint);
        currentIndex = Random.Range(0, wanderPoints.Length);
        canWander = true;
    }

    public void StartWandering()
    {
        canWander = true;
    }
}

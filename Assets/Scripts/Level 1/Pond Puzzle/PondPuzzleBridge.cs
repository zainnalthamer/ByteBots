using UnityEngine;

public class PondPuzzleBridge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LoopPuzzleValidator validator;
    [SerializeField] private Transform pond;
    [SerializeField] private Vector3 targetPosition = new Vector3(-109.57f, -0.1f, -124.16f);
    [SerializeField] private float moveSpeed = 1.5f;

    private Vector3 startPosition;
    private bool moving = false;

    private void Awake()
    {
        if (!validator)
            validator = GetComponent<LoopPuzzleValidator>();

        if (pond)
            startPosition = pond.position;
    }

    private void Update()
    {
        if (!moving && validator != null && validator.enabled && !validator.gameObject.activeSelf)
        {
        }
    }

    public void OnPuzzleSolved()
    {
        if (!moving)
            StartCoroutine(MovePond());
    }

    private System.Collections.IEnumerator MovePond()
    {
        moving = true;
        Vector3 start = pond.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            pond.position = Vector3.Lerp(start, targetPosition, t);
            yield return null;
        }

        pond.position = targetPosition;
        moving = false;
    }
}

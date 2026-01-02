using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Vector3 openLocalPosition;
    [SerializeField] private Vector3 openLocalRotation;
    [SerializeField] private float openSpeed = 1.5f;

    private bool isOpening = false;
    private Vector3 closedLocalPosition;
    private Vector3 closedLocalRotation;

    private void Start()
    {
        closedLocalPosition = transform.localPosition;
        closedLocalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (isOpening)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, openLocalPosition, Time.deltaTime * openSpeed);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, openLocalRotation, Time.deltaTime * openSpeed * 2);

            if (Vector3.Distance(transform.localEulerAngles, openLocalRotation) < 0.1f)
            {
                Debug.Log("[DoorController] Door fully opened!");
                isOpening = false;
            }
        }
    }

    public void OpenDoor()
    {
        Debug.Log("[DoorController] Opening door...");
        isOpening = true;
    }
}

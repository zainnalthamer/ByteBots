using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class AreaTrigger : MonoBehaviour
{
    [Tooltip("Tag of the object that should trigger the event.")]
    public string targetTag = "Player";

    [Tooltip("Event to invoke when the target enters the trigger.")]
    public UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onTriggerEnter?.Invoke();
        }
    }
}

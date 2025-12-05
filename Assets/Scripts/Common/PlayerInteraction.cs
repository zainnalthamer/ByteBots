using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;
    private InteractableObjects currentInteractable;

    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            InteractableObjects interactable = hit.collider.GetComponent<InteractableObjects>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    ClearCurrent();
                    currentInteractable = interactable;
                    currentInteractable.ShowPrompt();
                }
                return;
            }
        }

        ClearCurrent();
    }

    void ClearCurrent()
    {
        if (currentInteractable != null)
        {
            currentInteractable.HidePrompt();
            currentInteractable = null;
        }
    }
}

using UnityEngine;

public class NotebookTrigger : MonoBehaviour
{
    public NotebookController notebookController;
    public int panelToOpen = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            notebookController.OpenNotebook(panelToOpen);
        }
    }
}

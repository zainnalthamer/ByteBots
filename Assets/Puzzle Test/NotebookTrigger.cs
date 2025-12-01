using UnityEngine;

public class NotebookTrigger : MonoBehaviour
{
    public NotebookController notebookController;
    public int panelToOpen;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        notebookController.puzzlePanels[panelToOpen].SetActive(true);

        notebookController.OpenNotebook(panelToOpen);

        Debug.Log("Opened notebook panel: " + panelToOpen);
    }
}
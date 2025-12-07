using UnityEngine;

public class NotebookClickOpener : MonoBehaviour
{
    [Header("Notebook")]
    public NotebookController notebookController;
    public int panelIndex = 0;

    private void OnMouseDown()
    {
        if (notebookController == null)
        {
            Debug.LogWarning($"{name}: NotebookController is not assigned!");
            return;
        }

        notebookController.OpenNotebook(panelIndex);
    }
}

using UnityEngine;

public class NotebookTrigger : MonoBehaviour
{
    public NotebookController notebookController;
    public int panelIndex = 0;
    public GameObject openPrompt;

    bool playerInside = false;

    private void Start()
    {
        if (openPrompt != null)
            openPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            notebookController.OpenNotebook(panelIndex);

            if (openPrompt != null)
                openPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (openPrompt != null)
            openPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (openPrompt != null)
            openPrompt.SetActive(false);
    }
}

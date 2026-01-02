using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    public GameObject interactPrompt;
    public int panelIndex;

    public NotebookController notebookController;

    private bool isPlayerLooking = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public void ShowPrompt()
    {
        isPlayerLooking = true;
        if (interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        isPlayerLooking = false;
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerLooking && Input.GetKeyDown(KeyCode.E))
        {
            notebookController.OpenNotebook(panelIndex);
        }
    }
}


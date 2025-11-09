using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class LeverPuzzleInteraction : MonoBehaviour
{
    public Camera leverCamera;
    public Camera mainCamera;
    public GameObject player;
    public GameObject leverUI;
    public GameObject leverGroup;

    bool playerInRange = false;
    bool isInteracting = false;

    void Start()
    {
        leverCamera.gameObject.SetActive(false);
        if (leverGroup)
        {
            foreach (Collider col in leverGroup.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isInteracting) EnterInteraction();
            else ExitInteraction();
        }
    }

    void EnterInteraction()
    {
        isInteracting = true;
        leverCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);

        if (player.TryGetComponent(out CharacterController controller))
            controller.enabled = false;
        if (player.TryGetComponent(out ThirdPersonController tpc))
            tpc.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (leverUI) leverUI.SetActive(false);
        if (leverGroup)
        {
            foreach (Collider col in leverGroup.GetComponentsInChildren<Collider>())
                col.enabled = true;
        }
    }

    public void ExitInteraction()
    {
        isInteracting = false;
        leverCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        if (player.TryGetComponent(out CharacterController controller))
            controller.enabled = true;
        if (player.TryGetComponent(out ThirdPersonController tpc))
            tpc.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (leverGroup)
        {
            foreach (Collider col in leverGroup.GetComponentsInChildren<Collider>())
                col.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (leverUI) leverUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (leverUI) leverUI.SetActive(false);
            if (isInteracting) ExitInteraction();
        }
    }
}

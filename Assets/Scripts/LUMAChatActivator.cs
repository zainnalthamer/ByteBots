using UnityEngine;
using UnityEngine.InputSystem;

public class LUMAChatActivator : MonoBehaviour
{
    public GameObject chatUI;
    private bool playerNear = false;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            bool active = !chatUI.activeSelf;
            chatUI.SetActive(active);

            if (active)
            {
                playerInput.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                playerInput.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            chatUI.SetActive(false);
            playerInput.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

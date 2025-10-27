using UnityEngine;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Camera zoomCamera;
    public GameObject blocksUI;
    public Transform door;

    [Header("Door Target Transform")]
    public Vector3 openPosition = new Vector3(-0.796133f, 1.141755f, 0.7975853f);
    public Vector3 openRotation = new Vector3(0f, 113.047f, 0f);

    private string enteredCode = "";
    private bool playerNear = false;
    private bool configured = false;

    void Update()
    {
        if (!playerNear) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenBlocksUI();
        }

        if (Input.GetKeyDown(KeyCode.E) && configured)
        {
            TestCode();
        }
    }

    void OpenBlocksUI()
    {
        blocksUI.SetActive(true);
        zoomCamera.enabled = true;
        playerCamera.enabled = false;
    }

    public void CloseBlocksUI()
    {
        blocksUI.SetActive(false);
        zoomCamera.enabled = false;
        playerCamera.enabled = true;
        configured = true; 
        Debug.Log("Password configured. Ready to test.");
    }

    public void EnterDigit(string digit)
    {
        enteredCode += digit;
    }

    void TestCode()
    {
        if (enteredCode == PuzzleManager.Instance.setPassword)
        {
            Debug.Log("ACCESS GRANTED!");
            OpenDoor();
        }
        else
        {
            Debug.Log("ACCESS DENIED!");
            enteredCode = "";
        }
    }

    void OpenDoor()
    {
        door.position = openPosition;
        door.rotation = Quaternion.Euler(openRotation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = false;
    }

    public void OnPasswordConfigured()
    {
        configured = true;
        Debug.Log("Password configured via Run. UI closed and gameplay resumed.");
    }

}

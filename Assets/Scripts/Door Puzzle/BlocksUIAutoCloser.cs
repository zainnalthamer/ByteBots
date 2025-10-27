using UnityEngine;

public class BlocksUIAutoCloser : MonoBehaviour
{
    [Header("References")]
    public KeypadController keypad;
    public GameObject blocksUI;
    public Camera playerCamera;
    public Camera zoomCamera;

    public void CloseUIAfterRun()
    {
        blocksUI.SetActive(false);

        zoomCamera.enabled = false;
        playerCamera.enabled = true;

        if (keypad != null)
        {
            keypad.OnPasswordConfigured();
        }

        Debug.Log("Blocks UI closed after Run clicked.");
    }
}

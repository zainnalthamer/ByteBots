using UnityEngine;
using UnityEngine.UI;

public class BlocksPuzzleZone : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Root Canvas (or panel) that contains the Blocks Engine UI")]
    [SerializeField] private GameObject blocksCanvas;

    [Tooltip("The Play button inside the Blocks Engine UI")]
    [SerializeField] private Button playButton;

    [Header("Player Detection")]
    [Tooltip("Tag used to identify the player")]
    [SerializeField] private string playerTag = "Player";

    //[Header("Cursor & Control")]
    //[SerializeField] private bool lockCursorDuringUI = false;

    private bool uiOpen = false;

    private void Start()
    {
        Debug.Log("[BlocksPuzzleZone] Active and ready on " + gameObject.name);
    }

    private void Awake()
    {
        if (blocksCanvas != null)
        {
            blocksCanvas.SetActive(false);
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayClicked);
        }
        else
        {
            Debug.LogWarning("[BlocksPuzzleZone] Play Button is not assigned.");
        }
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(OnPlayClicked);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        ShowBlocksUI();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
    }

    private void ShowBlocksUI()
    {
        if (uiOpen) return;

        blocksCanvas.SetActive(true);
        uiOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var playerController = GameObject.FindWithTag(playerTag)?.GetComponent<MonoBehaviour>();
        if (playerController != null)
            playerController.enabled = false;
    }

    public void HideBlocksUI()
    {
        if (!uiOpen) return;

        blocksCanvas.SetActive(false);
        uiOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var playerController = GameObject.FindWithTag(playerTag)?.GetComponent<MonoBehaviour>();
        if (playerController != null)
            playerController.enabled = true;
    }

    private void OnPlayClicked()
    {
        HideBlocksUI();
    }
}

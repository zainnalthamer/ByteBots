using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BarnDoorTrigger : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup enterPanel;
    public Image fadePanel;

    [Header("Teleport")]
    public Transform teleportPoint;
    public Transform player;

    private bool playerInside;

    void Start()
    {
        enterPanel.alpha = 0f;
        enterPanel.interactable = false;
        enterPanel.blocksRaycasts = false;

        fadePanel.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            StartTransition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        enterPanel.DOFade(1f, 0.25f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        enterPanel.DOFade(0f, 0.25f);
    }

    void StartTransition()
    {
        enterPanel.DOFade(0f, 0.2f);

        fadePanel.DOFade(1f, 0.6f).OnComplete(() =>
        {
            player.SetPositionAndRotation(
                teleportPoint.position,
                teleportPoint.rotation
            );

            fadePanel.DOFade(0f, 0.6f);
        });
    }
}

using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BarnDoorTrigger : MonoBehaviour
{
    public CanvasGroup enterText;
    public Image fadePanel;
    public Transform teleportPoint;
    public Transform player;

    private bool playerInside = false;

    void Start()
    {
        enterText.alpha = 0f;
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
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            enterText.DOFade(1f, 0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            enterText.DOFade(0f, 0.3f);
        }
    }

    void StartTransition()
    {
        enterText.DOFade(0f, 0.2f);

        fadePanel.DOFade(1f, 0.7f).OnComplete(() =>
        {
            player.position = teleportPoint.position;
            player.rotation = teleportPoint.rotation;

            fadePanel.DOFade(0f, 0.7f);
        });
    }
}

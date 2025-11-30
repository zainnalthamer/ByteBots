using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PuzzleCardController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject root;
    [SerializeField] private Button iconButton;

    [Header("Events")]
    public UnityEvent OnLumaClicked;

    private void Awake()
    {
        if (!root) root = gameObject;

        if (canvas)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 60;
        }

        root.SetActive(false);

        if (iconButton != null)
        {
            iconButton.onClick.RemoveAllListeners();
            iconButton.onClick.AddListener(HandleLumaClick);
        }
        else
        {
            Debug.LogWarning("[PuzzleCardController] No iconButton assigned.");
        }
    }

    private void HandleLumaClick()
    {
        OnLumaClicked?.Invoke();
    }

    public void Show(string question, Sprite iconSprite = null)
    {
        if (questionText) questionText.text = question;
        if (icon && iconSprite) icon.sprite = iconSprite;

        root.SetActive(true); 
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}

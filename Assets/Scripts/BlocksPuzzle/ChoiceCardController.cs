using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceCardController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button explainButton;
    [SerializeField] private Button examplesButton;
    [SerializeField] private TMP_Text explainButtonText;
    [SerializeField] private TMP_Text examplesButtonText;
    [SerializeField] private GameObject root;

    private string currentConcept = "";

    private void Awake()
    {
        if (!root) root = gameObject;
        if (canvas)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 60;
        }
        root.SetActive(false);
    }

    public void Show(string concept, Sprite iconSprite = null)
    {
        currentConcept = concept;

        if (icon && iconSprite)
            icon.sprite = iconSprite;

        if (explainButtonText)
            explainButtonText.text = $"Explain {concept}";

        if (examplesButtonText)
            examplesButtonText.text = $"Give examples of {concept}";

        if (questionText)
            questionText.text = $"What do you want to know about {concept}?";

        root.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}

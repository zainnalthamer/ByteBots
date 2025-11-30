using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neocortex;
using DG.Tweening;

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
    [SerializeField] private GameObject choicesCardPanel;

    [Header("Neocortex References")]
    [SerializeField] private Transform chatContainer;
    [SerializeField] private Transform chatPanel;
    [SerializeField] private NeocortexTextChatInput chatInput;


    private string currentConcept = "";

    private void Awake()
    {
        if (!choicesCardPanel) choicesCardPanel = gameObject;
        if (canvas)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 60;
        }
        choicesCardPanel.SetActive(false);
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

        choicesCardPanel.SetActive(true);
    }

    public void Hide()
    {
        choicesCardPanel.SetActive(false);
    }

    #region Button Callbacks
    public void OnExplainButtonClicked()
    {
        
        var message = explainButtonText.text;
        Debug.Log(message);
        //AnimateChatPanel(true);
        chatInput.SendCustomMessage(message); 
    }

    public void OnExamplesButtonClicked()
    {
        var message = examplesButtonText.text;
        Debug.Log(message);
        //AnimateChatPanel(true);
        chatInput.SendCustomMessage(message);
    }

    //void AnimateChatPanel(bool animate)
    //{ 
    //    if(animate) 
    //        chatPanel.DOLocalMoveX(300, 1.5f).SetEase(Ease.OutQuad);
    //    else
    //        chatPanel.DOLocalMoveX(2222, 1.5f).SetEase(Ease.OutQuad);
    //}

    #endregion

}

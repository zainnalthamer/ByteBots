using UnityEngine;

public class ChoiceCardTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ChoiceCardController choiceCard;

    [Header("Settings")]
    [SerializeField] private string conceptName = "Variables";
    [SerializeField] private Sprite lumaIcon;

    private void Start()
    {
        if (choiceCard)
            choiceCard.Hide();

        OpenChoiceCard();
    }

    private void OpenChoiceCard()
    {
        if (!choiceCard) return;

        choiceCard.gameObject.SetActive(true);

        choiceCard.Show(conceptName, lumaIcon);
    }
}

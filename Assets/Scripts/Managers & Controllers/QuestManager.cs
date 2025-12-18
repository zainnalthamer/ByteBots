using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest Panel Root")]
    [SerializeField] private GameObject questPanel;

    [Header("Quest UI")]
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Image questImage;
    [SerializeField] private Image questSticker;
    [SerializeField] private TextMeshProUGUI questNumberText;

    [Header("Quest Data (ALL SAME SIZE)")]
    [TextArea][SerializeField] private string[] questTexts;
    [SerializeField] private Sprite[] questImages;
    [SerializeField] private Sprite[] questStickers;
    [SerializeField] private int[] questNumbers;

    private int currentQuestIndex = -1;
    private bool questVisible;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        questPanel.SetActive(false);
        ShowNextQuest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ToggleQuest();
    }

    public void ShowNextQuest()
    {
        currentQuestIndex++;

        if (currentQuestIndex >= questTexts.Length)
            return;

        ApplyQuestByIndex(currentQuestIndex);
    }

    private void ApplyQuestByIndex(int index)
    {
        questText.text = questTexts[index];

        if (questImages.Length > index && questImage)
            questImage.sprite = questImages[index];

        if (questStickers.Length > index && questSticker)
            questSticker.sprite = questStickers[index];

        if (questNumbers.Length > index && questNumberText)
            questNumberText.text = questNumbers[index].ToString();

        questPanel.SetActive(true);
        questVisible = true;
    }

    public void OnPuzzleCompleted()
    {
        StartCoroutine(NextQuestDelay());
    }

    private IEnumerator NextQuestDelay()
    {
        yield return new WaitForSeconds(3f);
        ShowNextQuest();
    }

    private void ToggleQuest()
    {
        questVisible = !questVisible;
        questPanel.SetActive(questVisible);
    }
}

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
    [SerializeField] private TextMeshProUGUI questNumberText;

    [Header("Quest Blur")]
    [SerializeField] private GameObject questBlurVolume;

    [Header("Quest Data (same index = same quest)")]
    [TextArea][SerializeField] private string[] questTexts;
    [SerializeField] private Sprite[] questImages;
    [SerializeField] private string[] questNumbers; 

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
        if (questBlurVolume) questBlurVolume.SetActive(false);

        //ShowNextQuest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ToggleQuest();
    }

    //public void ShowNextQuest()
    //{
    //    currentQuestIndex++;

    //    if (currentQuestIndex >= questTexts.Length)
    //        return;

    //    ApplyQuestByIndex(currentQuestIndex);
    //}

    public void ShowQuestByIndex(int index)
    {
        if (index < 0 || index >= questTexts.Length)
            return;

        currentQuestIndex = index;
        ApplyQuestByIndex(index);
    }

    private void ApplyQuestByIndex(int index)
    {
        questText.text = questTexts[index];

        if (questImages.Length > index && questImage)
            questImage.sprite = questImages[index];

        if (questNumbers.Length > index && questNumberText)
            questNumberText.text = questNumbers[index];

        questPanel.SetActive(true);
        questVisible = true;

        if (questBlurVolume)
            questBlurVolume.SetActive(true);
    }

    public void OnPuzzleCompleted(int nextQuestIndex)
    {
        StartCoroutine(NextQuestDelay(nextQuestIndex));
    }

    private IEnumerator NextQuestDelay(int index)
    {
        yield return new WaitForSeconds(10f);
        ShowQuestByIndex(index);
    }

    private void ToggleQuest()
    {
        questVisible = !questVisible;
        questPanel.SetActive(questVisible);

        if (questBlurVolume)
            questBlurVolume.SetActive(questVisible);
    }
}

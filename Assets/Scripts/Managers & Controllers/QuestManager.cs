using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestObjects
    {
        public string questName;
        public GameObject[] selectableObjects;
    }

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

    public int CurrentQuestIndex => currentQuestIndex;

    [Header("Quest : Objects")]
    [SerializeField] private QuestObjects[] questObjects;

    [Header("Quest Auto Hide")]
    [SerializeField] private float autoHideDelay = 5f;


    [Header("DEV / TEST CONTROLS")]
    [SerializeField] private bool enableDebugKeys = true;

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

        int savedQuest = SaveManager.I.GetCurrentQuestIndex();

        if (savedQuest >= 0)
        {
            ForceShowQuest(savedQuest);
        }
        else
        {
            ShowNextQuest();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            ToggleQuest();

        if (!enableDebugKeys) return;

        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            ForceShowQuest(currentQuestIndex + 1);

        if (Input.GetKeyDown(KeyCode.KeypadDivide))
            ForceShowQuest(currentQuestIndex - 1);
    }

    public void ShowNextQuest()
    {
        currentQuestIndex++;

        if (currentQuestIndex >= questTexts.Length)
            return;

        ApplyQuestByIndex(currentQuestIndex);
        UpdateSelectableObjects();
    }

    public void ShowQuestByIndex(int index)
    {
        if (SaveManager.I.IsQuestCompleted(index))
            return;

        currentQuestIndex = index;
        ApplyQuestByIndex(index);
        UpdateSelectableObjects();
    }

    private void ForceShowQuest(int index)
    {
        if (index < 0 || index >= questTexts.Length)
        {
            Debug.Log("[QuestManager] Invalid quest index: " + index);
            return;
        }

        currentQuestIndex = index;

        Debug.Log("[QuestManager] FORCED quest index: " + currentQuestIndex);

        ApplyQuestByIndex(currentQuestIndex);
        UpdateSelectableObjects();
    }

    private void ApplyQuestByIndex(int index)
    {
        if (questText) questText.text = questTexts[index];

        if (questImages.Length > index && questImage)
            questImage.sprite = questImages[index];

        if (questNumbers.Length > index && questNumberText)
            questNumberText.text = questNumbers[index];

        questPanel.SetActive(true);
        questVisible = true;

        if (questBlurVolume)
            questBlurVolume.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AutoHideQuest());
    }

    public void OnPuzzleCompleted(int nextQuestIndex)
    {
        SaveManager.I.MarkQuestCompleted(currentQuestIndex);
        SaveManager.I.SaveQuestProgress(nextQuestIndex);
        StartCoroutine(NextQuestDelay(nextQuestIndex));
    }

    private IEnumerator NextQuestDelay(int index)
    {
        yield return new WaitForSeconds(10f);
        ShowQuestByIndex(index);
    }

    private void ToggleQuest()
    {
        StopAllCoroutines();

        questVisible = !questVisible;
        questPanel.SetActive(questVisible);

        if (questBlurVolume)
            questBlurVolume.SetActive(questVisible);
    }

    public bool IsCurrentQuest(int questIndex)
    {
        return currentQuestIndex == questIndex;
    }

    private void UpdateSelectableObjects()
    {
        if (questObjects == null || questObjects.Length == 0)
            return;

        foreach (var quest in questObjects)
        {
            if (quest == null || quest.selectableObjects == null) continue;

            foreach (var obj in quest.selectableObjects)
            {
                if (!obj) continue;

                if (obj.CompareTag("Selectable"))
                    obj.tag = "Untagged";
            }
        }

        if (currentQuestIndex < 0 || currentQuestIndex >= questObjects.Length)
            return;

        if (questObjects[currentQuestIndex] == null || questObjects[currentQuestIndex].selectableObjects == null)
            return;

        foreach (var obj in questObjects[currentQuestIndex].selectableObjects)
        {
            if (!obj) continue;

            obj.tag = "Selectable";
        }
    }

    private IEnumerator AutoHideQuest()
    {
        yield return new WaitForSeconds(autoHideDelay);

        questVisible = false;
        questPanel.SetActive(false);

        if (questBlurVolume)
            questBlurVolume.SetActive(false);
    }

}
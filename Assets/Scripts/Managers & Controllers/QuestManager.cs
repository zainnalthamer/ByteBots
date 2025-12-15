using System.Collections;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI questText;

    [Header("Quests")]
    [TextArea]
    [SerializeField] private string[] quests;

    private int currentQuestIndex = -1;
    private bool questVisible = false;

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
        ShowNextQuest(); // start first quest
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleQuest();
        }
    }

    public void ShowNextQuest()
    {
        currentQuestIndex++;

        if (currentQuestIndex >= quests.Length)
            return;

        questText.text = quests[currentQuestIndex];
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

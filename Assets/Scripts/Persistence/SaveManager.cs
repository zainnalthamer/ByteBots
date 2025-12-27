using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager I { get; private set; }

    [SerializeField] private string saveFile = "save.es3";

    private int bugCount = 30;
    private HashSet<string> completedLevels = new();
    private HashSet<string> solvedPuzzles = new();

    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;

    private int currentQuestIndex = -1;
    private HashSet<int> completedQuests = new();

    public int GetCurrentQuestIndex() => currentQuestIndex;
    public bool IsQuestCompleted(int questIndex) => completedQuests.Contains(questIndex);

    public static bool IsNewGame = false;


    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (IsNewGame)
        {
            ResetAll();
            bugCount = 30;
            ES3.Save(SaveKeys.BugCount, bugCount, saveFile);
            IsNewGame = false;
        }
        else
        {
            LoadAll();
        }
    }

    public int GetBugCount() => bugCount;
    public bool IsLevelCompleted(string scene) => completedLevels.Contains(scene);
    public bool IsPuzzleSolved(string puzzleId) => solvedPuzzles.Contains(puzzleId);

    public void IncrementBugCount()
    {
        bugCount++;
        ES3.Save(SaveKeys.BugCount, bugCount, saveFile);
    }

    public void MarkLevelCompleted(string sceneName = null)
    {
        sceneName ??= SceneManager.GetActiveScene().name;
        if (completedLevels.Add(sceneName))
            ES3.Save(SaveKeys.CompletedLevels, new List<string>(completedLevels), saveFile);
    }

    public void MarkPuzzleSolved(string puzzleId)
    {
        if (string.IsNullOrEmpty(puzzleId)) return;
        if (solvedPuzzles.Add(puzzleId))
            ES3.Save(SaveKeys.SolvedPuzzles, new List<string>(solvedPuzzles), saveFile);
    }

    private void LoadAll()
    {
        if (ES3.KeyExists(SaveKeys.BugCount, saveFile))
            bugCount = ES3.Load<int>(SaveKeys.BugCount, saveFile);

        if (ES3.KeyExists(SaveKeys.CompletedLevels, saveFile))
            completedLevels = new HashSet<string>(ES3.Load<List<string>>(SaveKeys.CompletedLevels, saveFile));

        if (ES3.KeyExists(SaveKeys.SolvedPuzzles, saveFile))
            solvedPuzzles = new HashSet<string>(ES3.Load<List<string>>(SaveKeys.SolvedPuzzles, saveFile));

        if (ES3.KeyExists(SaveKeys.CurrentQuestIndex, saveFile))
            currentQuestIndex = ES3.Load<int>(SaveKeys.CurrentQuestIndex, saveFile);

        if (ES3.KeyExists(SaveKeys.CompletedQuests, saveFile))
            completedQuests = new HashSet<int>(
                ES3.Load<List<int>>(SaveKeys.CompletedQuests, saveFile)
            );
    }

    public void ResetAll()
    {
        bugCount = 30;
        completedLevels.Clear();
        solvedPuzzles.Clear();
        completedQuests.Clear();
        currentQuestIndex = -1;

        ES3.DeleteFile(saveFile);
    }

    public void IncrementBugCountBy(int n)
    {
        bugCount += n;
        bugCount = Mathf.Max(0, bugCount);
        ES3.Save(SaveKeys.BugCount, bugCount, saveFile);
    }

    public bool HasPlayerTransform()
    {
        return ES3.KeyExists(SaveKeys.PlayerPosition, saveFile)
            && ES3.KeyExists(SaveKeys.PlayerRotation, saveFile);
    }

    public Vector3 GetPlayerPosition()
    {
        return ES3.Load<Vector3>(SaveKeys.PlayerPosition, saveFile);
    }

    public Quaternion GetPlayerRotation()
    {
        return ES3.Load<Quaternion>(SaveKeys.PlayerRotation, saveFile);
    }

    public void SavePlayerTransform(Transform player)
    {
        lastPlayerPosition = player.position;
        lastPlayerRotation = player.rotation;

        ES3.Save(SaveKeys.PlayerPosition, lastPlayerPosition, saveFile);
        ES3.Save(SaveKeys.PlayerRotation, lastPlayerRotation, saveFile);
    }

    public void SaveQuestProgress(int questIndex)
    {
        currentQuestIndex = questIndex;
        ES3.Save(SaveKeys.CurrentQuestIndex, currentQuestIndex, saveFile);
    }

    public void MarkQuestCompleted(int questIndex)
    {
        if (completedQuests.Add(questIndex))
            ES3.Save(
                SaveKeys.CompletedQuests,
                new List<int>(completedQuests),
                saveFile
            );
    }

}
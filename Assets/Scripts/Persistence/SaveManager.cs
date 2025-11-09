using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager I { get; private set; }

    [SerializeField] private string saveFile = "save.es3";

    private int bugCount = 0;
    private HashSet<string> completedLevels = new();
    private HashSet<string> solvedPuzzles = new();

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        LoadAll();
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
    }

    public void ResetAll()
    {
        bugCount = 0;
        completedLevels.Clear();
        solvedPuzzles.Clear();
        ES3.DeleteFile(saveFile);
    }

    public void IncrementBugCountBy(int n)
    {
        bugCount += Mathf.Max(0, n);
        ES3.Save(SaveKeys.BugCount, bugCount, saveFile);
    }
}

using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    public LocalData localData = new LocalData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("MuteSound", localData.muteSound ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        localData.muteSound = PlayerPrefs.GetInt("MuteSound", 0) == 1;
    }
}

[System.Serializable]
public class LocalData
{
    public bool muteSound = false;
}

using UnityEngine;

public class PlayerAutoSave : MonoBehaviour
{
    void OnApplicationQuit()
    {
        SaveManager.I.SavePlayerTransform(transform);
    }

    void OnDisable()
    {
        if (SaveManager.I != null)
            SaveManager.I.SavePlayerTransform(transform);
    }
}

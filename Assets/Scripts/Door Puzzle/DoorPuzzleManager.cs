using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    [HideInInspector] public string setPassword = "";

    void Awake()
    {
        Instance = this;
    }

    public void SetPassword(string newPassword)
    {
        setPassword = newPassword;
        Debug.Log("Password configured: " + newPassword);
    }
}

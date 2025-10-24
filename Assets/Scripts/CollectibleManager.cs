using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    [Header("UI Elements")]
    public TMP_Text chipCounterText;

    private int chipsCollected = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddChip()
    {
        chipsCollected++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (chipCounterText)
            chipCounterText.text = "Chips: " + chipsCollected;
    }
}

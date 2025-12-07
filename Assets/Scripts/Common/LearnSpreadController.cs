using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnSpreadController : MonoBehaviour
{
    public GameObject[] spreads;
    int currentIndex = 0;

    void OnEnable()
    {
        currentIndex = 0;
        ShowCurrent();
    }

    public void NextSpread()
    {
        if (currentIndex < spreads.Length - 1)
        {
            currentIndex++;
            ShowCurrent();
        }
    }

    public void PreviousSpread()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowCurrent();
        }
    }

    void ShowCurrent()
    {
        for (int i = 0; i < spreads.Length; i++)
        {
            spreads[i].SetActive(i == currentIndex);
        }
    }
}
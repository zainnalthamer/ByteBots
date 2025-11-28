using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {


    public GameObject PressPlayText;
    public GameObject ToSetActive;

    public GameObject ButtonPressedText;
    public GameObject LeverUsedText;
    public GameObject SwitchUsedText;
    public GameObject ValveUsedText;

    public float DisableCounter;
    public bool TextActive = false;

    private void Start()
    {
        PressPlayText.SetActive(false);
        ToSetActive.SetActive(true);
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ToSetActive.SetActive(false);
        }

        if(TextActive == true)
        {
            DisableCounter += 1 * Time.deltaTime;
        }

        if(DisableCounter > 2)
        {
            ButtonPressedText.SetActive(false);
            LeverUsedText.SetActive(false);
            SwitchUsedText.SetActive(false);
            ValveUsedText.SetActive(false);
            DisableCounter = 0;
        }


    }

    public void OnButtonPressed()
    {
        ButtonPressedText.SetActive(true);
        TextActive = true;
    }

    public void OnLeverUsed()
    {
        LeverUsedText.SetActive(true);
        TextActive = true;
    }

    public void OnSwitchUsed()
    {
        SwitchUsedText.SetActive(true);
        TextActive = true;
    }

    public void OnValveUsed()
    {
        ValveUsedText.SetActive(true);
        TextActive = true;
    }

}


/*
    Made by Quint van Oorschot (Monqo Games)

    Thank you for using the 'Action and reaction pack'!
    You can use is for both commercial and personal projects.

    Read the 'READ ME!.txt' to learn how to use this package

    Do you need support? E-mail: monqogames@gmail.com
    */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ValveScript : MonoBehaviour {


    [Header("Play function when the valve is being used:")]
    public UnityEvent WhenValveIsBeingUsed;
    [Header("Play function when 'CurrentValue' is increasing (Holding left mousebutton):")]
    public UnityEvent WhenCurrentValueIsIncreasing;
    [Header("When 'CurrentValue' equals the value of 'MaxValue'")]
    public UnityEvent WhenCurrentValueIsMax;
    [Header("Play function when 'CurrentValue' is higher than 'X'")]
    public float X;
    public UnityEvent WhenCurrentValueIsHigherThanX;
    [Header("Play function when 'CurrentValue' is lower than 'y'")]
    public float Y;
    public UnityEvent WhenCurrentValueIsLowerThanY;


    [Header("General Settings")]
    public bool UseAnimations;
    public bool UseSoundEffects;

    [Header("Important Variables")]
    public float CurrentValue = 0.0f; //Use valve to increase this (ranges from 0 to 'MaxValue')
    public float MaxValue = 100.0f; //The highest value possible of 'Value'
    public float IncreasementPerSecond = 1.0f; //Increasement of 'Value' per second

    [Header("Debug")]
    public bool LogCurrentValueInConsole; //Debug.Log the value of 'CurrentValue'
    [Space(3)]
    public bool LogWhenValveIsBeingUsed; //Debug.Log when the valve is being used;
    public string PrintMessage = "The valve is being used!";

    [Header("Animation")]
    public Transform ObjectToRotate;

    AudioSource ValveSfx;
    bool AudioPlaying = false;
    float PlayingCounter;


    private void Start()
    {
        ValveSfx = this.GetComponent<AudioSource>();
    }

    void Update()
    {


        if (LogCurrentValueInConsole == true)
        {
            Debug.Log(CurrentValue);
        }

        if (CurrentValue > X)
        {
            WhenCurrentValueIsHigherThanX.Invoke();
        }

        if (CurrentValue < Y)
        {
            WhenCurrentValueIsLowerThanY.Invoke();
        }

        if (AudioPlaying == true)
        {
            PlayingCounter += 1 * Time.deltaTime;
        }

        if (PlayingCounter > 1.8f)
        {
            AudioPlaying = false;
            PlayingCounter = 0;
        }

        if (CurrentValue > MaxValue) //'CurrentValue' can't be higher than 'MaxValue'
        {
            CurrentValue = MaxValue;
            WhenCurrentValueIsMax.Invoke();
        }

        if (CurrentValue < 0) //'CurrentValue' can't be lower than zero
        {
            CurrentValue = 0;
        }

    }

    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) //Play sound effect if the mouse button has been pressed
        {
            if (AudioPlaying == false) //If soundeffect is not already playing
            {
                if (UseSoundEffects == true)
                {
                    ValveSfx.Play();
                    AudioPlaying = true;
                }
            }
        }


        /*Hold left mousebutton to increase 'CurrentValue' and hold the right mousebutton to decrease
         * 'CurrentValue' (if using default settings). */

        //0 = Left mousebutton || 1 = Right mousebutton || 2 = Middle mousebutton
        if (Input.GetMouseButton(0))
        {

            if (LogWhenValveIsBeingUsed == true)
            {
                Debug.Log(PrintMessage);
            }

            WhenValveIsBeingUsed.Invoke();
            WhenCurrentValueIsIncreasing.Invoke();
            CurrentValue += IncreasementPerSecond * Time.deltaTime;

            if (UseAnimations == true)
            {
                ObjectToRotate.Rotate(Vector3.forward, 10 * Time.deltaTime);
            }
        }

        if (Input.GetMouseButton(1))
        {
            WhenValveIsBeingUsed.Invoke();
            CurrentValue -= IncreasementPerSecond * Time.deltaTime;
            if (UseAnimations == true)
            {
                if (ObjectToRotate != null)
                {
                    ObjectToRotate.Rotate(Vector3.forward, -10 * Time.deltaTime);
                }
            }
        }


    }
}

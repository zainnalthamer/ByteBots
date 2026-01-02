using System.Collections;

/*
    Made by Quint van Oorschot (Monqo Games)

    Thank you for using the 'Action and reaction pack'!
    You can use is for both commercial and personal projects.

    Read the 'READ ME!.txt' to learn how to use this package

    Do you need support? E-mail: monqogames@gmail.com
    */

using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{


    [Header("Play function when the button is pressed:")]
    public UnityEvent WhenButtonIsPressed;
    [Header("Play function when user hovers cursor over the button")]
    public UnityEvent WhenCursorHoversOverButton;
    [Header("If the button is pressed more times than the value of 'x'")]
    public int X; //Has to be assigned
    public UnityEvent WhenTimesPressedIsHigherThanX;
    [Header("Important variables")]
    public int TimesUsed; //Howmany times the button has been pressed
    [Header("General Settings:")]
    public bool UseAnimations; //Do you want to use the default animations?
    public bool UseSoundEffects; //Do you want to play sound effects?
    [Space(5)]
    [Header("Debug")]
    public bool PrintWhenButtonPressed = false; //THIS SETTING IS UP TO YOU!
    public string PrintMessage = "Buttton is pressed!"; //How the message should be printed
    [Space(3)]
    public Transform ToDrawLineTo; //THIS SETTING IS UP TO YOU!
    [Header("Fun settings")]
    public bool ChangeColorWhenPressed = false; //THIS SETTING IS UP TO YOU!


    //Sets to 'true' when button is pressed
    public static bool ButtonPressed = false;

    //Different colors for when button is pressed
    public Color StartColor;
    public Color PressedColor;

    Animator anim; //The Animator which starts the animation when 'ButtonPressed' equals true
    AudioSource PressedSoundEffect; //Play sound effect when the button is pressed
    public GameObject toAnimObject; //The GameObject which contains the parts which move because of the 'Animator'
    bool AnimationPlaying = false;
    float AnimationCounter;

    void Start()
    {

        anim = toAnimObject.GetComponent<Animator>(); //Connect with the 'Animator' in the 'toAnimObject'
        PressedSoundEffect = this.GetComponent<AudioSource>(); //Connect with the 'Audio Source'


    }


    private void OnMouseOver() //If cursor hovers over the a collider of the button
    {

        WhenCursorHoversOverButton.Invoke();

        //0 = Left mousebutton || 1 = Right mousebutton || 2 = Middle mousebutton
        if (Input.GetMouseButtonDown(0)) //If pressed
        {

            TimesUsed += 1;


                if (TimesUsed > X)
                {
                    WhenTimesPressedIsHigherThanX.Invoke();
                }
            

            if (PrintWhenButtonPressed == true)
            {
                Debug.Log(PrintMessage);
            }


            WhenButtonIsPressed.Invoke();


            if (UseAnimations == true)
            {
                anim.Play("ButtonPressedAnimation"); //Play this animation once
            }

            if (AnimationPlaying == false)
            { //If it is not playing already

                if (UseSoundEffects == true) //Only play sound effects if the creator wants to
                {
                    PressedSoundEffect.Play(); //Play the sound effect
                    AnimationPlaying = true; //Let the script now that the Animation is playing
                }
            }



            if (ChangeColorWhenPressed == true)
            {
                toAnimObject.GetComponent<Renderer>().material.color = PressedColor;
            }

        }

    }

    private void Update()
    {

        if (ToDrawLineTo != null) //If 'ToDrawLineTo' has a value assigned
        {
            Debug.DrawLine(gameObject.transform.position, ToDrawLineTo.position); //Draws a line from the button to 'WhereToDrawLine'
        }


        //This resets the script to it's normal state when the animation is finished           
        if (AnimationPlaying == true)
        {
            AnimationCounter += 1 * Time.deltaTime; //Add 1 to 'AnimationCounter' every second
        }

        if (AnimationCounter > 0.9f) //If animation is finished
        {
            anim.Play("Idle"); //Set the animator to idlee
            AnimationCounter = 0; //Reset the value of 'AnimationCounter'
            AnimationPlaying = false; //Let the code know that the animation isn't playing

            if (ChangeColorWhenPressed == true) //If the value of 'ChangeColorWhenPressed' equals true
            {
                toAnimObject.GetComponent<Renderer>().material.color = StartColor;
            }

        }

    }

}

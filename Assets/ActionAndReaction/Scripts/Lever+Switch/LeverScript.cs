
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

public class LeverScript : MonoBehaviour {

    [Header("Play function when the lever used:")]
    public UnityEvent PlayWhenLeverUsed;
    [Header("Play function when 'leverstate' equals true:")]
    public UnityEvent WhenLeverStateEqualsTrue;
    [Header("Play function when 'leverstate' equals false:")]
    public UnityEvent WhenLeverStateEqualsFalse;
    [Header("Play function when the cursor hovers over the lever:")]
    public UnityEvent WhenHoversOverLever;
    [Header("Important Variables:")]
    public bool LeverState = false; //True = on  || False = off
    [Space(5)]
    [Header("Important Variable:")]
    public bool UseAnimations;
    public bool UseSoundEffects;
    [Header("Animamtor settings:")]
    public GameObject StickGameObject; //The object which moves because of the animation
    [Header("Debug:")]
    public bool PrintLeverState;
    [Space(5)]
    public Transform ToDrawLineTo;

    //----------------------------------------------------------------------------------------------------------------------------------------

    bool AnimationPlaying = false; //false when game starts
    float AnimationCounter; //Resets script to default state when the animation is finished
    AudioSource SoundEffect; //The sound effect when the lever is used
    Animator anim;


    private void Start() //When script starts for the first time
    {
        anim = StickGameObject.GetComponent<Animator>();
        SoundEffect = this.GetComponent<AudioSource>();
    }


    private void OnMouseOver() //If cursor moves over a collider
    {

        WhenHoversOverLever.Invoke();

        //0 = Left mousebutton || 1 = Right mousebutton || 2 = Middle mousebutton ||
        if (Input.GetMouseButtonDown(0)) //If player presses mouse button
        {
            if (AnimationPlaying == false) //If nothing is moving already
            {
                PlayWhenLeverUsed.Invoke();

                if (LeverState == false)
                {
                    LeverState = true;

                    if (UseSoundEffects == true) //Only play sound effects if the creator wants to
                    {
                        SoundEffect.Play();
                    }

                    if (UseAnimations == true) //Only use animations if the creator wants to
                    {
                        anim.Play("LeverOnAnimation");
                        AnimationPlaying = true;
                    }

                }
                else
                {
                    LeverState = false;

                    if (UseSoundEffects == true)
                    {
                        SoundEffect.Play();
                    }

                    if (UseAnimations == true) //Only use animations if the creator wants to
                    {
                        anim.Play("LeverOffAnimation");
                        AnimationPlaying = true;
                    }

                }

            }

        }
    }

    private void Update()
    {

        if(ToDrawLineTo != null)
        {

            Debug.DrawLine(gameObject.transform.position, ToDrawLineTo.position);
        }

        if(PrintLeverState == true)
        {
            Debug.Log(LeverState);

            if(ToDrawLineTo == null)
            {
                Debug.LogError("Where to draw to line to has not been assigned!");
            }

        }


        if (LeverState == true)
        {
            WhenLeverStateEqualsTrue.Invoke();
        }
        else
        {
            WhenLeverStateEqualsFalse.Invoke();
        }

        if (AnimationPlaying == true)
        {
            AnimationCounter += 1 * Time.deltaTime;
        }

        if (AnimationCounter > 1.5f)
        {
            AnimationCounter = 0;
            AnimationPlaying = false;
        }

    }
}

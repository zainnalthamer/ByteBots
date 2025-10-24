
/*
    Made by Quint van Oorschot (Monqo Games)

    Thank you for using the 'Ultimate interaction pack'!
    You can use is for both commercial and personal projects.

    I hope this helps you with your next game!

    Read the 'READ ME!.txt' to learn how to use this package

    Regards, Quint van Oorschot

    */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {
    

    [Header("Enter the tag of the object that can trigger the plate:")]
    public string CollisionTag; //Enter the tag of the object of collision
    [Header("Play function once when 'CollisionTag' touches the pressure plate:")]
    public UnityEvent WhenCollisionEnters; //The assigned functions will run once when the collision started
    [Header("Play function every frame as long as the object touches the plate:")]
    public UnityEvent WhenCollisionStay; //The assigned functions will run every frame until the collision stops
    [Header("Play function once when the objects stops touching the plate:")]
    public UnityEvent WhenCollisionExits; //The assigned functions will run when the collision ends
    [Header("Settings:")]
    public bool UseSoundEffects = true; //Do you want to use sound effects?
    public bool UseAnimations = true; //Do you want to use animations?
    [Header("Debug:")]
    public bool PrintOnCollision = false; //Print a message in the console which prints the value of 'PrintMessage'
    public string PrintMessage = "Collision detected!"; //Which message should be printed? (If 'PrintOnCollision' is enabled)

    Animator anim;
    AudioSource Sfx;


    void Start () {

        Sfx = this.GetComponent<AudioSource>(); //Assign the value of 'Sfx' when the script starts
        anim = this.GetComponent<Animator>(); //Assign the value of 'anim' when the script starts

        if (CollisionTag == "") //Send a message when no value of 'CollisioinTag' has been assigned
        {
            Debug.LogError("The value of 'CollisionTag' has not been assigned!"); //Prints the message
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        
        if(PrintOnCollision == true) //If the value of 'PrintOnCollision' equals True
        {
            Debug.Log(PrintMessage); //Prints the message
        }

        if(collision.gameObject.tag == CollisionTag) //If the collision object has a tag which equals the value of 'CollisionTag'
        {

            if (UseSoundEffects == true) //If the value of 'UseSoundEffects' equals true
            {
                Sfx.Play(); //Plays the sound effect of the pressure plate
            }

            if (UseAnimations == true) //If the value of 'UseAnimations' equals true
            {
                anim.Play("PressurePlateEnterAnimation"); //Plays the animation of pressure plate
            }
            WhenCollisionEnters.Invoke(); //Runs all assigned functions once when the collision starts
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        
        if(collision.gameObject.tag == CollisionTag) //If the collision object has a tag which equals the value of 'CollisionTag'
        {
            WhenCollisionStay.Invoke(); //Runs all assigned functions every frame until the collision ends
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        
        if(collision.gameObject.tag == CollisionTag) //If the collision object has a tag which equals the value of 'CollisionTag'
        {

            if (UseSoundEffects == true) //If the value of 'UseSoundEffects' equals true
            {
                Sfx.Play(); //Plays the sound effect of the pressure plate
            }

            if (UseAnimations == true) //If the value of 'UseAnimations' equals true  
            {
                anim.Play("PressurePlateReleaseAnimation"); //Plays the animation of pressure plate
            }
            WhenCollisionExits.Invoke(); //Runs all assigned functions once when the collision ends
        }

    }


}

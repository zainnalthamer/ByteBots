
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

public class SwitchScript : MonoBehaviour {


    [Header("Play function when the switch is being used")]
    public UnityEvent WhenSwitchIsBeingUsed;
    [Header("Play function when the value of 'SwitchState' equals True")]
    public UnityEvent WhenSwitchStateEqualsTrue;
    [Header("Play function when the value of 'SwitchState' equals False")]
    public UnityEvent WhenSwitchStateEqualsFalse;

    [Header("General Settings:")]
    public bool SwitchState = false; //false = off || true = on

    [Header("Get difference look for different values of 'LeverState'")]
    public GameObject OffStateObject;
    public GameObject OnStateObject;

    AudioSource SoundEffect; //Sound effect


    private void Start()
    {
        SoundEffect = this.GetComponent<AudioSource>(); //Connect the Audio Source with the script
    }

    private void Update() //Runs every frame
    {
        
        if(SwitchState == false) //If the value of 'SwitchState' equals false
        {
            WhenSwitchStateEqualsFalse.Invoke(); //Runs all assigned functions when the value of 'SwitchState' equals false
        }

        if (SwitchState == true) //If the value of 'SwitchState' equals true
        {

            WhenSwitchStateEqualsTrue.Invoke(); //Runs all assigned functions when the value of 'SwitchState' equals true
        }

    }

    private void OnMouseOver() //if the player hovers the cursor over the 'Box Collider' the switch
    {

        //0 = Left mousebutton || 1 = Right mousebutton || 2 = Middle mousebutton ||
        if (Input.GetMouseButtonDown(0))
        {

            WhenSwitchIsBeingUsed.Invoke(); //Run the 'IfObjectIsPressed' void

            SoundEffect.Play(); //Play sound effect

            if(SwitchState == false)
            {
                    if (OffStateObject != null) //Check if 'OffStateObject' is not nothing
                    {
                        if (OnStateObject != null) //Check if 'OnStateObject' is not nothing
                        {
                            //Change the look of the switch
                            OffStateObject.SetActive(false);
                            OnStateObject.SetActive(true);
                        }
                    }
                SwitchState = true;
            }
            else
            {
                    if (OffStateObject != null) //The GameObjects must be assigned
                    {
                        if (OnStateObject != null)
                        {
                            //Change the look of the switch
                            OffStateObject.SetActive(true);
                            OnStateObject.SetActive(false);
                        }
                    }
                SwitchState = false;
            }

        }

    }

}

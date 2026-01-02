using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class StoryManager : MonoBehaviour
{

    public GameObject cutsceneCameras;
    public GameObject cutsceneObject;
    public bool firstGame = true; 
    public Flowchart introFlowchart;

    [Header("TicketBooth Setting")]
    public BoxCollider ticketBoothCollider;

    // Start is called before the first frame update
    void Awake()
    {

        if(PlayerPrefs.HasKey("firstGame"))
        {
            firstGame = false;
            cutsceneCameras.SetActive(false);
            Destroy(cutsceneObject);
        }  
        else
        {
            cutsceneCameras.SetActive(true);
            PlayerPrefs.SetString("firstGame", "yes");
            SetupIntroScene();
        }

        if (PlayerPrefs.HasKey("CarnivalGateOpened"))
            ticketBoothCollider.enabled = false;

    }
     


    void SetupIntroScene()
    {
        introFlowchart.ExecuteBlock("IntroCutscene");
    }
}

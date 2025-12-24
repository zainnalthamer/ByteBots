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

    // Start is called before the first frame update
    void Awake()
    {
        if(ES3.KeyExists("firstGame"))
        {
            firstGame = false;
            cutsceneCameras.SetActive(false);
            Destroy(cutsceneObject);
        }
        else
        {
            cutsceneCameras.SetActive(true);
            ES3.Save<bool>("firstGame", true);
            SetupIntroScene();
        }
    }
     


    void SetupIntroScene()
    {
        introFlowchart.ExecuteBlock("IntroCutscene");
    }
}

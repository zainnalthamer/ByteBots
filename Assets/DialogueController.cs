using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    [Header("Fungus Cutscene")]
    [SerializeField] private Fungus.Flowchart flowchart;
    [SerializeField] private string blockNameToPlay = "PickJars";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if (flowchart != null)
            {
                flowchart.ExecuteBlock(blockNameToPlay);
            }
        }
    }
}

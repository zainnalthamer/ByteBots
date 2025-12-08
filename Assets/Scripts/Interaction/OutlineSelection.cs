using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outlines>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outlines>() != null)
                {
                    highlight.gameObject.GetComponent<Outlines>().enabled = true;
                }
                else
                {
                    Outlines outline = highlight.gameObject.AddComponent<Outlines>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outlines>().OutlineColor = Color.white;
                    highlight.gameObject.GetComponent<Outlines>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outlines>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outlines>().enabled = true;
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    selection.gameObject.GetComponent<Outlines>().enabled = false;
                    selection = null;
                }
            }
        }
    }

}
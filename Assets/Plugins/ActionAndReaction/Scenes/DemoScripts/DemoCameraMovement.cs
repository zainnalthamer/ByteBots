using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCameraMovement : MonoBehaviour {

    public GameObject UseKeysText;

    void Update () {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-1 * Time.deltaTime, 0, 0);
            UseKeysText.SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(1 * Time.deltaTime, 0, 0);
            UseKeysText.SetActive(false);
        }

    }
}

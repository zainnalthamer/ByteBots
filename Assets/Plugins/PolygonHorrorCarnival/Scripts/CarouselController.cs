using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselController : MonoBehaviour
{
    [Header("Ride Speed")]
    [Range(-30, 30)]
    public float rideSpeed = -12.0f;

    [Header("Base Platform")]
    public GameObject Platform;

    [Header("Cranks")]
    public Transform[] Cranks;

    [Header("Control Settings")]
    public bool isActive = false; 
    private float currentSpeed = 0f; 

    void Start()
    {
        currentSpeed = 0f;
    }

    void Update()
    {
        if (!isActive) return;

        currentSpeed = Mathf.Lerp(currentSpeed, rideSpeed, Time.deltaTime * 1.5f);

        Platform.transform.Rotate(Vector3.up * currentSpeed * Time.deltaTime);

        foreach (Transform crank in Cranks)
        {
            crank.Rotate(Vector3.forward * (currentSpeed * 1.25f) * Time.deltaTime * 10);
        }
    }

    public void ActivateCarousel()
    {
        isActive = true;
        Debug.Log("Carousel started!");
    }
}

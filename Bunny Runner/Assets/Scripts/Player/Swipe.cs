using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Player prefab
//Sends swipe info to PlayerController.cs
public class Swipe : MonoBehaviour
{
    //Access to controller
    PlayerController playerControllerScript;

    private float firstSwipePosition;

    private const float MaxSwipeDistance = 0.2f;  //max swipe percentage per screen


    private void Awake()
    {
        playerControllerScript = this.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSwipe();
    }
    private void CheckSwipe()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstSwipePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;       
        }
        else if(Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            CheckSwipeThreshold();           
        }


    }

    private void CheckSwipeThreshold()
    {
        float distance = MeasureSwipeLength();

        if (Mathf.Abs(distance) >= MaxSwipeDistance)
        {
            playerControllerScript.AssignTargetPositionY(Mathf.Sign(distance));
        }
       
    }

    public float MeasureSwipeLength()
    {
        float lastSwipePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
        return lastSwipePosition - firstSwipePosition;
    }

}

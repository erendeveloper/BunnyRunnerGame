using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Player prefab
//Sends swerve input to PlayerController.cs"
//How it works: There is a box a box collider on Player prefab.Sends rays through the collider and checks its x point;It's layer is called "Swerve"
public class Swerve : MonoBehaviour
{
    //Access to contrroller
    PlayerController playerControllerScript;

    private Camera mainCamera;

    LayerMask swerveLayerMask;

    private float raycastDistance = 10f;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerControllerScript = this.GetComponent<PlayerController>();
        swerveLayerMask = LayerMask.GetMask("Swerve");
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
           SendRay();
    }
    private void SendRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, raycastDistance, swerveLayerMask))
        {
            playerControllerScript.AssignTargetPositionX(hit.point.x);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Roads Prefab
//After player pass on the road, it moves to the end
public class RoadsController : MonoBehaviour
{
    private float lastRoadPositionZ;
    private float roadSizeZ;
    private float targetDistance;//distance to move
    private const int totalLane = 3;
    public int TotalLane { get => totalLane; }
    private const float laneDistance = 0.3f;
    public float LaneDistance {get => laneDistance;} //distances between lanes

    private void Awake()
    {
        AssignRoadValues();
    }
 
    private void AssignRoadValues() 
    {
        int roadCount = transform.childCount;
        Transform lastRoad = transform.GetChild(roadCount - 1);
        lastRoadPositionZ = lastRoad.position.z;
        roadSizeZ = lastRoad.localScale.z;
        targetDistance = roadCount * roadSizeZ * 10f;
    }

    public void MoveRoad(Transform road)//adds distance to last road position on every move
    {
        lastRoadPositionZ = road.localPosition.z + targetDistance;
        road.localPosition = new Vector3(road.localPosition.x, road.localPosition.y, lastRoadPositionZ);
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Road Prefab
public class Road : MonoBehaviour
{
    //Access the controller
    private RoadsController roadsControllerScript;
    //Access other script
    private ItemGenerator itemGeneratorScript;

    [SerializeField] private bool isFirstRoad;//dont't instantiate items on first roads
    private void Awake()
    {
        roadsControllerScript = transform.parent.GetComponent<RoadsController>();
        itemGeneratorScript = transform.parent.GetComponent<ItemGenerator>();
    }
    private void Start()//burasi awake idi, item generator icinden cagirabil;irsin
    {
        if (!isFirstRoad)
        {        
            itemGeneratorScript.RandomFillArea(this.transform);
        }
        isFirstRoad = false;
    }

    private void OnTriggerEnter(Collider other)//Player (Collider Swerve inside of it) triggers
    {      
        foreach(Item item in GetComponentsInChildren<Item>()) //move objects to the pool
        {
            item.DisableObject();
            itemGeneratorScript.AddItemToPool(item);
        }
        roadsControllerScript.MoveRoad(this.transform);
        itemGeneratorScript.RandomFillArea(this.transform);//regenerates items on road after moving
    }
}

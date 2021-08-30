using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

//Added on Roads GameObject
//Generates obstacles and collectables and settle them again with object pooling
public class ItemGenerator : MonoBehaviour
{
    //Access controller
    public RoadsController roadsControllerScript;

    public List<GameObject> allItemPrefabs = new List<GameObject>();//drag prefabs on inspector

    private List<List<Item>> itemsWithSize = new List<List<Item>>();//categorize items by their size

    private List<int> lanesToBeUsed = new List<int>();//Used for 1 size items to select the lane to put on

    private int totalCollectable=0; //Automatically calculated

    private int collectablePercent=50;//chance to generate collactables over 100

    //object pooling
    private List<List<GameObject>> prefabPool = new List<List<GameObject>>();


    private void Awake()
    {
        AssignAllItemPrefabs();
        AssignLanes();
        CreateInstancesInPrefabPool();
    }
    #region assign empty lists
    private void AssignAllItemPrefabs()
    {

        for (int i = 1; i <= roadsControllerScript.TotalLane; i++)
        {
            //Creates sub empty lists, sizes-1 are indexes
            List<Item> sameSizeList = new List<Item>();
            itemsWithSize.Add(sameSizeList);
        }

        foreach (GameObject prefab in allItemPrefabs)
        {
            int size = prefab.GetComponent<Item>().Size;
            itemsWithSize[size - 1].Add(prefab.GetComponent<Item>());//categorize items by their size

            if(size==1 && prefab.GetComponent<Item>().Type == ItemType.Collectable)
            {
                totalCollectable++;
            }
        }

    }
    private void AssignLanes()//just assigns lane numbers
    {
        lanesToBeUsed.Clear();
        for (int i = 1; i <= roadsControllerScript.TotalLane; i++)
        {
            lanesToBeUsed.Add(i);
        }
    }
    private void CreateInstancesInPrefabPool()//Creates sub empty lists, sizes-1 are indexes
    {
        foreach (GameObject prefab in allItemPrefabs)
        {
            List<GameObject> sameGameObjectList = new List<GameObject>();
            prefabPool.Add(sameGameObjectList);
        }
    }
    #endregion


    public void RandomFillArea(Transform road)//main generation function
    {
        int totalSize = Random.Range(1, roadsControllerScript.TotalLane+1);
        RandomSelectItems(totalSize, road);

    }
    public void RandomSelectItems(int remainingArea, Transform road)
    {
        if (remainingArea <= 0) //empty, currently not used, can be used if wanted
        {
            return;
        }
        else if (remainingArea == 1)
        { 
            int randomLane = Random.Range(0, lanesToBeUsed.Count);               
            Generate(remainingArea,lanesToBeUsed[randomLane],road);
            AssignLanes();
        }
        else if(remainingArea == 2)
        {          
            int randomPart = Random.Range(1,remainingArea+1);
            if (randomPart == 2) //2 items with size 1
            {
                remainingArea--;
                int randomLane = Random.Range(0, lanesToBeUsed.Count);                  
                Generate(remainingArea, lanesToBeUsed[randomLane], road);
                lanesToBeUsed.RemoveAt(randomLane);
                RandomSelectItems(remainingArea,road);
            }
            else //1 item with size 2
            {
                Generate(remainingArea, (roadsControllerScript.TotalLane + 1) / 2, road);
            }
        }
        else if (remainingArea == 3)
        {
            Generate(remainingArea, (roadsControllerScript.TotalLane+1)/2, road);
        }

    }

    private void Generate(int size, int lane, Transform road)
    {
        bool selectCollectable = false;
        int randomCollectable = Random.Range(0, 100);

        int totalItemWithSize;
        int randomItemWithSize;
        if (size == 1 && randomCollectable < collectablePercent)
        {
            selectCollectable = true;
            randomItemWithSize = Random.Range(0,totalCollectable);
        }
        else if(size==1)
        {
            totalItemWithSize = itemsWithSize[size - 1].Count;
            randomItemWithSize = Random.Range(totalCollectable,totalItemWithSize-totalCollectable);
            
        }
        else
        {
            totalItemWithSize = itemsWithSize[size - 1].Count;
            randomItemWithSize = Random.Range(0, totalItemWithSize);
        }

        GameObject newGameObject;

        bool spawn = true;
        if (selectCollectable)
        {
            foreach(GameObject gameobject in prefabPool[0])
            {
                if (gameobject.GetComponent<Item>().Type == ItemType.Collectable)
                    spawn = false;              
            }          
        }
        else if(size==1)
        {
            foreach (GameObject gameobject in prefabPool[0])
            {
                if (gameobject.GetComponent<Item>().Type == ItemType.Obstacle)
                    spawn = false;
            }
        }
        else if(prefabPool[randomItemWithSize].Count != 0)
        {
            spawn = false;
        }

        if (spawn)
        {

            GameObject prefab;
            if (selectCollectable)
            {
                prefab = allItemPrefabs[randomItemWithSize];
            }
            else
            {
                prefab = itemsWithSize[size - 1][randomItemWithSize].gameObject;  
            }           
            newGameObject = Instantiate(prefab, new Vector3(0, 0, 0), prefab.transform.rotation);
        }
        else //take from pool
        {          
            if (selectCollectable)
            {
                List<int> currentCollactableIndexList = new List<int>();
                for(int i=0; i< prefabPool[0].Count; i++)
                {
                    if (prefabPool[0][i].GetComponent<Item>().Type == ItemType.Collectable)
                    {
                        currentCollactableIndexList.Add(i);
                    }
                }
                randomItemWithSize = currentCollactableIndexList[Random.Range(0,currentCollactableIndexList.Count)];
            }
            else if (size == 1)
            {
                List<int> currentObstacleIndexList = new List<int>();
                for (int i = 0; i < prefabPool[0].Count; i++)
                {
                    if (prefabPool[0][i].GetComponent<Item>().Type == ItemType.Obstacle)
                    {
                        currentObstacleIndexList.Add(i);
                    }
                }
                randomItemWithSize = currentObstacleIndexList[Random.Range(0, currentObstacleIndexList.Count)];
            }
            else
            {
                randomItemWithSize = 0; //first elemet of list
                //prefab = itemsWithSize[size - 1][randomItemWithSize].gameObject;
            }

            newGameObject = prefabPool[size -1][randomItemWithSize];
            prefabPool[size - 1].RemoveAt(randomItemWithSize);
            //newGameObject.transform.parent = null;
        }
        newGameObject.transform.parent = road;
        float lanePositionX = CalculateLanePosition(lane,size);
        newGameObject.transform.localPosition = new Vector3(lanePositionX, 0, 0);
        newGameObject.SetActive(true);
    }

    private float CalculateLanePosition(int lane,int size)
    {
        float positionX;
        float laneDistance = roadsControllerScript.LaneDistance * 10f;//becasue it is local, multiply by 10
        if (size == 2)
        {
            int random = Random.Range(0, 2);
            int sign=1;
            if(random == 1)
            {
                sign *= -1;
            }
            positionX = (laneDistance / 2f) * sign;
         }
        else
        {
            positionX = (lane - 2) * laneDistance;
        }
        return positionX;
    }

    public void AddItemToPool(Item item)
    {
        Transform transform = item.transform;
        transform.parent = null;
        prefabPool[item.Size].Add(transform.gameObject);
    }
}

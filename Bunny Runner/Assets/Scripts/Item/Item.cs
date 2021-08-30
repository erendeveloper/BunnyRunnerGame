using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on items which are prefabs
public class Item : MonoBehaviour
{
    //Access other scripts
    GameManager gameManagerScript;
    PlayerController playerControllerScript;

    Rigidbody itemRigidbody;
    Collider itemCollider;

    Vector3 force =new Vector3(2f,4f,2f);//For throwing on punch

    [SerializeField] private int size;
    public int Size { get => size; }

    [SerializeField] public ItemType type;
    public ItemType Type { get => type; }

    private void Awake()
    {
        gameManagerScript = Camera.main.GetComponent<GameManager>();
        playerControllerScript = GameObject.FindGameObjectWithTag("Character").GetComponent<PlayerController>();

        itemRigidbody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();

    }
   
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Rabbit")) // do not trigger with other items
        {
            if (type == ItemType.Collectable)
            {
                gameManagerScript.GainScore(1);
                transform.gameObject.SetActive(false);
            }
            else//obstacle
            {
                playerControllerScript.CrashObstacle();
                if (gameManagerScript.LevelUp)
                {
                    Throw();
                }
                
            }
        }
        
    }
   
    private void Throw()
    {
        itemRigidbody.isKinematic = false;
        int randomWay = Random.Range(0, 2);//0 changes way X to opoosite
        if (randomWay == 0)
        {
            force.x *= -1;
        }
        itemCollider.enabled = false;
        itemRigidbody.AddForce(force, ForceMode.VelocityChange);
        Invoke("DisableObject",3f);//falling in 3 seconds
    }

    public void DisableObject()
    {
        itemRigidbody.isKinematic = true;
        itemCollider.enabled = true;
        itemRigidbody.velocity = Vector3.zero;
        transform.gameObject.SetActive(false);
    }

}

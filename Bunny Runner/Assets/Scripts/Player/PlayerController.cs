using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Player prefab
//Works with Swipe.cs and Swerve.cs
public class PlayerController : MonoBehaviour
{
    //Access other scripts
    Swipe swipeScript;
    GameManager gameManagerScript;

    private bool isPlayerAlive = true;

    public Transform bodyHorizontal; //player body for horizontal movement
    public Transform bodyVertical;   //player body for vertical movement

    //Rabbit Transforms
    public Transform normalRabbit;
    public Transform muscleRabbit;
    
    //Animators of rabbits
    private Animator normalRabbitAnimator;
    private Animator muscleRabbitAnimator;

    //for movements of body parts of the player
    private Vector3 targetPositionX = Vector3.zero;
    private Vector3 targetPositionY = Vector3.zero;

    private bool isPlayerOnGround = true;

    //Player's Y positions
    private float onGroundPositionY = 0f;
    private float underGroundPositionY = -0.18f;

    private float speedForward = 1f;

    private float smoothDampTimeX = 0.1f;
    private float smoothDampTimeY = 0.15f;

    //reference velocities for SmoothDamp
    private Vector3 smoothDampVelocityX = Vector3.zero;
    private Vector3 smoothDampVelocityY = Vector3.zero;

    private const float MaxHorizontalDistance = 0.3f;//Leftest and rightest distance from the middle //burasi ile roads kontroldeki distance ayni


    private void Awake()
    {
        swipeScript = this.GetComponent<Swipe>();
        gameManagerScript = Camera.main.GetComponent<GameManager>();
        normalRabbitAnimator = normalRabbit.GetComponent<Animator>();
        muscleRabbitAnimator = muscleRabbit.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isPlayerAlive)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speedForward);

            MoveVertically();
            MoveHorizontally();
        }   
        
    }

    private void MoveVertically()
    {      
        bodyVertical.localPosition = Vector3.SmoothDamp(bodyVertical.localPosition, targetPositionY, ref smoothDampVelocityY, smoothDampTimeY);

        if (Vector3.Distance(bodyVertical.localPosition, targetPositionY) < 0.001f)
        {
            bodyVertical.localPosition = targetPositionY;
        }
    }
    
    private void MoveHorizontally()
    {
        bodyHorizontal.localPosition = Vector3.SmoothDamp(bodyHorizontal.localPosition, targetPositionX, ref smoothDampVelocityX, smoothDampTimeX);

        if (Vector3.Distance(bodyHorizontal.localPosition, targetPositionX) < 0.01f)
        {
            bodyHorizontal.localPosition = targetPositionX;
        }
    }
    public void AssignTargetPositionX(float targetVectorX)//input comes from Serve.cs
    {
        targetPositionX.x = Mathf.Clamp(targetVectorX, -MaxHorizontalDistance, MaxHorizontalDistance);
    }
    public void AssignTargetPositionY(float way)//input comes from Swipe.cs
    {
        if (isPlayerOnGround)
        {
            if (way < 0)
            {
                targetPositionY.y = underGroundPositionY;
                isPlayerOnGround = false;
            }

        }
        else
        {
            if (way > 0)
            {
                targetPositionY.y = onGroundPositionY;
                isPlayerOnGround = true;
            }
        }
    }

    public void BecomeMuscleRabbit()
    {
        normalRabbit.gameObject.SetActive(false);
        muscleRabbit.gameObject.SetActive(true);
    }
    public void BecomeNormalRabbit()
    {
        muscleRabbit.gameObject.SetActive(false);
        normalRabbit.gameObject.SetActive(true);
        
    }

    public void CrashObstacle()//On Trigger Enter obstacles
    {
        if (gameManagerScript.LevelUp)
        {
            muscleRabbitAnimator.SetBool("Punch", true);
        }
        else
        {            
            isPlayerAlive = false;
            normalRabbitAnimator.SetTrigger("Death");
            gameManagerScript.Die();
        }
    }
    
}

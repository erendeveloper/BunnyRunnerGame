using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Added on Main Camera
public class GameManager : MonoBehaviour
{
    //Access other script
    public PlayerController playerControllerScript;
    public Swipe swipeScript;
    private UIController uiControllerScript;

    private int score=0; //each carrot collected gives 1 point
    public int Score { get => score; }
    private int levelUpCount = 0; //5 makes the rabbit myscle
    float levelUpTime = 7f;

    private bool levelUp = false; //if true, becomes muscle
    public bool LevelUp { get => levelUp; }

    private void Awake()
    {
        uiControllerScript = GetComponent<UIController>();
    }

    public void GainScore(int amount)
    {
        score += amount;
        if (!levelUp)
        {
            levelUpCount++;
            CheckLevelUpCount();
        }
            
    }
    private void CheckLevelUpCount()
    {
        if (levelUpCount == 5)
        {
            levelUpCount = 0;
            ToggleLevelUp();
        }
    }

    public void ToggleLevelUp()
    {
        if (!levelUp)
        {
            levelUp = true;
            playerControllerScript.BecomeMuscleRabbit();
            swipeScript.enabled = false;
            Invoke("ToggleLevelUp", levelUpTime);//return to normal in LevelUpTime
        }
        else
        {
            levelUp = false;
            swipeScript.enabled = true;
            playerControllerScript.BecomeNormalRabbit();
        }
    }

    public void Die()
    {
        uiControllerScript.OpenGameOverMenu();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}

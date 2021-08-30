using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Added on Main Camera
public class UIController : MonoBehaviour
{
    //Access GameManager
    GameManager gameManagerScript;

    public Canvas gameOverCanvas;

    public Text score;

    private void Awake()
    {
        gameManagerScript = GetComponent<GameManager>();
    }

    public void OpenGameOverMenu()
    {
        gameOverCanvas.gameObject.SetActive(true);
        score.text = gameManagerScript.Score.ToString();
    }
    
}

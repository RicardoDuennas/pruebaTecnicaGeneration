using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{

   public GameObject GameOverMenuC;
    public GameObject restartGame;
    
    // Start is called before the first frame update
    void Start()
    {
        
        GameOverMenuC.SetActive(false);
        restartGame.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        
        GameOverMenuC.SetActive(true);
        restartGame.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void HideGameOVerMenu()
    {
        
        GameOverMenuC.SetActive(false);
        restartGame.SetActive(false);
        Time.timeScale = 1.0f;
    }
    

   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{

   public GameObject GameOverMenuC;
   public GameObject winMenu;
    public GameObject restartGame;
    
    // Start is called before the first frame update
    void Start()
    {

    }
    public void ShowWinMenu()
    {
        
        GameOverMenuC.SetActive(false);
        restartGame.SetActive(true);
        winMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
    
    public void HideWinMenu()
    {
        
        GameOverMenuC.SetActive(false);
        restartGame.SetActive(false);
        winMenu.SetActive(false);
        Time.timeScale = 1.0f;

    }
}

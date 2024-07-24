using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text timer;
    public GameOverMenu GameOverMenu;
    
    public float time = 5;

   
    // Update is called once per frame
    void Update()
    {
        timer.text = ($" {time:F2}");

        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                GameOverMenu.ShowGameOverMenu();
            }

        }
        // time / 60F: Divides the time in seconds (time) by 60 to convert it to minutes. The F suffix indicates that the number is of type float.
        int minutes = Mathf.FloorToInt(time / 60F);
        // time % 60F: Calculates the remainder of the division of time by 60. This gives the seconds remaining after accounting for the minutes.
        int seconds = Mathf.FloorToInt(time % 60F);
        // This function formats a string according to a specified pattern.
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


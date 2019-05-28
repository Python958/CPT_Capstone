using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DLC_TimerWinCondition : MonoBehaviour
{

    public Text ClockUI;
    public float startTime;
    private bool clockIsActive = true;
 
    // Start is called before the first frame update
    void Start()
    {
        startTime = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (clockIsActive)
        {
            float t = startTime - Time.timeSinceLevelLoad; //This is the countdown. Start Time - the time since the scene started. 
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2"); //"f2" is the format for 2 decimal places, we can always change if need be. 

            ClockUI.text = minutes + ":" + seconds; //This is the how the information will be displayed on the GUI

            //Win Condition Check
            if (t <= 0)
            {
                clockIsActive = false;
                Debug.Log("You Win");
                SceneManager.LoadScene("MainMenu");
            }
        }



    }
    
}

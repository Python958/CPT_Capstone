using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AMS_DisplayScoreRanks : MonoBehaviour
{
    public Text[] display;
    public string[] saveNames;
    public bool WereScoresDeleted = false;
    
   
    // Start is called before the first frame update
    void Start()
    {
    
        for (int i = 0; i < display.Length; i++)
        {
            if (PlayerPrefs.HasKey(saveNames[i]))
            {
                Debug.Log(PlayerPrefs.GetString(saveNames[i]));
                display[i].text = PlayerPrefs.GetString(saveNames[i]);
             //   Debug.Log(display[i].text);
            }
            else
            {
                display[i].text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (WereScoresDeleted == true)
        {
            AMS_ScoreController.GetScoreRank();
            Debug.Log("Ranks should disappear");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void DeleteScores()
    {
        for (int i = 0; i < display.Length; i++)
        {
            PlayerPrefs.DeleteKey(saveNames[0]);
            PlayerPrefs.DeleteKey(saveNames[1]);
            PlayerPrefs.DeleteKey(saveNames[2]);
            PlayerPrefs.DeleteKey(saveNames[3]);
            PlayerPrefs.DeleteKey(saveNames[4]);
            PlayerPrefs.DeleteKey(saveNames[5]);
            PlayerPrefs.DeleteKey(saveNames[6]);
            PlayerPrefs.DeleteKey(saveNames[7]);
        }
        if(FindObjectOfType<MainMenu>() != null)
        {
            FindObjectOfType<MainMenu>().ClickSound();
        }
        //Debug.Log(saveNames[0] + "has been deleted");
        //PlayerPrefs.DeleteAll();
        WereScoresDeleted = true;
       Debug.Log("The Player's scores has been deleted");
    }
}

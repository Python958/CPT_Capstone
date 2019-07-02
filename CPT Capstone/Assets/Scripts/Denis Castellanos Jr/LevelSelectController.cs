using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public AMS_DisplayScoreRanks DisplayScoreRanks;
    // public AMS_ResultsControl ResultsControl;

    public Text[] unlockText;
    public Button Button_ClearScores;
    public Text[] allLevels;
    public Button[] allButtons;
   
    public bool unlockArena = true;


    // Start is called before the first frame update
    void Start()
    {
        DisplayScoreRanks = FindObjectOfType<AMS_DisplayScoreRanks>();
        //ResultsControl = FindObjectOfType<AMS_ResultsControl>();
        foreach (Button button in allButtons)
        {
            if (button != allButtons[0])
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      AbilityToClearScore();
      UnlockLevels();
      UnlockingButtons();
    }

    void AbilityToClearScore()
    {
        if (allLevels[0].text == "")
      {
            Button_ClearScores.interactable = false;
           // Debug.Log("There is are no scores to delete");
      }
      else
      {
            Button_ClearScores.interactable = true;
            if (DisplayScoreRanks.WereScoresDeleted == true)
            {
                Button_ClearScores.interactable = false;
              //  Debug.Log("This button should be greyed out");
            }
      }
    }
    void UnlockLevels()
    {
        unlockArena = true;
        foreach(Text score in allLevels)
        {
            if (score != allLevels[0] && score != allLevels[7])
            {
                if (score.text != "SS" && score.text != "S" && score.text != "A")
                {
                    unlockArena = false;
                }
            }
        }
        if (unlockArena)
        {
            allButtons[7].interactable = true;
        }
        else
        {
            allButtons[7].interactable = false;
        }

    }

    void UnlockingButtons()
    {
       for (int i = 0; i < allLevels.Length; i++)
        {
            if (allLevels[i].text != "" && allLevels[i].text != "D" && allLevels[i].text != "F")
            {
                if (i != allLevels.Length && i != allLevels.Length - 1 && i != allLevels.Length - 2)
                {
                    allButtons[i + 1].interactable = true;
                    unlockText[i].enabled = false;
                    Debug.Log("Turn On" + (i + 1));
                }
            }
            if (allLevels[i].text == "")
            {
                if (i != allLevels.Length - 1 && i != allLevels.Length - 2)
                {
                    allButtons[i + 1].interactable = false;
                    Debug.Log("Turn Off " + (i + 1));
                }

            }
        }
    }
}

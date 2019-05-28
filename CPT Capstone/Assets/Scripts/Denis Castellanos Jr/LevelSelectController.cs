using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public AMS_DisplayScoreRanks DisplayScoreRanks;
    // public AMS_ResultsControl ResultsControl;

    public Button Button_Tutorial;
    public Button Button_Wave;
    public Button Button_TheFortress;
    public Button Button_TheHiveLevel;
    public Button Button_MidLevel;
    public Button Button_TheSkyFalls;
    public Button Button_ArenaCombat;
    public Button Button_ClearScores;

    public Text Rank_Tutorial;
    public Text Rank_Wave;
    public Text Rank_TheFortress;
    public Text Rank_TheHiveLevel;
    public Text Rank_MidLevel;
    public Text Rank_TheSkyFalls;
    public Text Rank_ArenaCombat;

    public Text[] unlockText;

    private Text[] levelsToCheck;
    private Text[] allLevels;
    private Button[] allButtons;
   
    public bool unlockArena = true;


    // Start is called before the first frame update
    void Start()
    {
        DisplayScoreRanks = FindObjectOfType<AMS_DisplayScoreRanks>();
        //ResultsControl = FindObjectOfType<AMS_ResultsControl>();

        Button_Tutorial.interactable = true;
        Button_Wave.interactable = false;
        Button_TheFortress.interactable = false;
        Button_TheHiveLevel.interactable = false;
        Button_MidLevel.interactable = false;
        Button_TheSkyFalls.interactable = false;
        Button_ArenaCombat.interactable = false;
        Button_ClearScores.interactable = false;

        Rank_Tutorial.GetComponent<Text>();
        Rank_Wave.GetComponent<Text>();
        Rank_TheFortress.GetComponent<Text>();
        Rank_TheHiveLevel.GetComponent<Text>();
        Rank_MidLevel.GetComponent<Text>();
        Rank_TheSkyFalls.GetComponent<Text>();
        Rank_ArenaCombat.GetComponent<Text>();
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
        if (Rank_Tutorial.text == "")
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
        /*     //All levels are locked until there is a tutorial score
          if (Rank_Tutorial.text == "")
          {
              Button_Wave.interactable = false;
              Button_TheFortress.interactable = false;
              Button_TheHiveLevel.interactable = false;
              Button_MidLevel.interactable = false;
              Button_TheSkyFalls.interactable = false;
              Button_ClearScores.interactable = false;
          }
       else if (Rank_Tutorial.text != "")
          {
              Button_Wave.interactable = true;
              Button_TheFortress.interactable = true;
              Button_TheHiveLevel.interactable = true;
              Button_MidLevel.interactable = true;
              Button_TheSkyFalls.interactable = true;
              Button_ClearScores.interactable = true;
          }
     */   
     //Arena Bonus is locked until As are achieved
        levelsToCheck = new Text[] {Rank_Wave, Rank_TheHiveLevel, Rank_TheSkyFalls};
        unlockArena = true;
        foreach(Text score in levelsToCheck)
        {
            if (score.text != "SS" && score.text != "S" && score.text != "A")
            {
                unlockArena = false;
            }
        }
        if (unlockArena)
        {
            Button_ArenaCombat.interactable = true;
        }

    }

    void UnlockingButtons()
    {
        allLevels = new Text[] { Rank_Tutorial, Rank_Wave, Rank_TheHiveLevel, Rank_TheSkyFalls, Rank_ArenaCombat };
        allButtons = new Button[] { Button_Tutorial, Button_Wave, Button_TheHiveLevel, Button_TheSkyFalls, Button_ArenaCombat };

       for (int i = 0; i < allLevels.Length; i++)
        {
            if (allLevels[i].text != "" && allLevels[i].text != "D" && allLevels[i].text != "F")
            {
                if (i != allLevels.Length && i != allLevels.Length - 1 && i != allLevels.Length - 2)
                {
                    allButtons[i + 1].interactable = true;
                    unlockText[i].enabled = false;
                   // Debug.Log(i);
                }
            }
            if (allLevels[i].text == "")
            {
                if (i != 0 && i != allLevels.Length - 1 && i != allLevels.Length - 2)
                {
                    allButtons[i + 1].interactable = false;
                   // Debug.Log("Turn Off" + i);
                }

            }
        }
    }
}

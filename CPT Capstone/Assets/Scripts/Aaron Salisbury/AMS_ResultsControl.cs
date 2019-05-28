using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AMS_ResultsControl : MonoBehaviour
{
    [Tooltip("Order Original Score, Par Time, Time Taken, Time Bonus Earned, Final Score, Rank")]
    public Text[] resultText;
    public GameObject goTolevelSelectButton;

    private string saveLevelInfo;
    private int oldRank;
    private int newRank;
    private float moreInfoTime = .5f;
    private int currentDisplay = 0;
    public AudioClip[] musicOptions;
    private AudioSource myAudioSource; 

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            myAudioSource = GetComponent<AudioSource>();
            if (AMS_ScoreController.victory)
            {
                myAudioSource.clip = musicOptions[0];
                myAudioSource.loop = true;
            }
            else
            {
                myAudioSource.clip = musicOptions[1];
                myAudioSource.loop = false;
            }
            myAudioSource.Play();
        }
        Cursor.visible = true;      
        if (AMS_ScoreController.victory)
        {
            resultText[4].enabled = false;
        }
    }

    private void Update()
    {
        moreInfoTime -= Time.deltaTime;
        if (moreInfoTime <= 0)
        {
            if (currentDisplay != 6)
            moreInfoTime = 1.5f;
            DisplayNewInfo(currentDisplay);
            currentDisplay++;
        }
    }

    public void GoToLevelSelect()
    {
        Debug.Log("gotolevelselect");
        SceneManager.LoadScene("levelSelect");
    }

    //Save the score value
    private void SaveScore()
    {
        saveLevelInfo = "Score " + AMS_ScoreController.lastSceneName;
        //Check that the currently saved score is not higher
        if (PlayerPrefs.HasKey(saveLevelInfo))
        {
            if (CheckRating())
            {
                Debug.Log("New is higher");
                PlayerPrefs.SetString(saveLevelInfo, AMS_ScoreController.GetScoreRank());
            }
            else
            {
                Debug.Log("Old is higher");
            }
        }
        else
        {
            Debug.Log("First time saving here");
            PlayerPrefs.SetString(saveLevelInfo, AMS_ScoreController.GetScoreRank());
        }
    }

    //Checks to see if current or previosu rank is higher
    private bool CheckRating()
    {
        Debug.Log("compare");
        for (int i = 0; i < AMS_ScoreController.letterRanks.Length; i++)
        {
            if (PlayerPrefs.GetString(saveLevelInfo) == AMS_ScoreController.letterRanks[i])
            {
                oldRank = i;
            }
            if (AMS_ScoreController.GetScoreRank() == AMS_ScoreController.letterRanks[i])
            {
                newRank = i;
            }
            //Incase a non valaid rank is earned it can be overwritten.
            if (PlayerPrefs.GetString(saveLevelInfo) == "" || PlayerPrefs.GetString(saveLevelInfo) == "6")
            {
                return true;
            }
        }
        if (oldRank < newRank)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    private void DisplayNewInfo(int i)
    {
        //Original Score
        if (i == 0)
        {
            resultText[0].text = "Score earned: " + AMS_ScoreController.score;
        }
        //Par Time
        if (i == 1)
        {
            resultText[1].text = "Par Time: " + ((int)AMS_ScoreController.parTimes[0] / 60).ToString() + ":" + (AMS_ScoreController.parTimes[0] % 60).ToString("f0");
        }
        //Time Taken
        if (i == 2)
        {
            resultText[2].text = "Time Taken: " + ((int)AMS_ScoreController.timeTaken / 60).ToString() + ":" + (AMS_ScoreController.timeTaken % 60).ToString("f0");
        }
        //Time Bonus Earned
        if (i == 3)
        {
            AMS_ScoreController.CompareTimeToPar();
            resultText[3].text = "Time Bonus Earned: " + AMS_ScoreController.timeBonusEarned;
        }
        //Failure Text
        if (i == 4)
        {
            if (!AMS_ScoreController.victory)
            {
                resultText[4].text = "Score Failure Multiplier: 0%";
                AMS_ScoreController.score = 0;
            }
            else
            {
                moreInfoTime = 0;
            }
        }
        //Final Score
        if (i == 5)
        {
            resultText[5].text = "Final Score: " + AMS_ScoreController.score;
        }
        //Rank
        if (i == 6)
        {
            resultText[6].text = AMS_ScoreController.GetScoreRank();
            SaveScore();
            goTolevelSelectButton.SetActive(true);
        }
    }
}

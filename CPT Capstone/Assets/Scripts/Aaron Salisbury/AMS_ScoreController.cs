using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AMS_ScoreController
{
    public static int score = 0;
    public static float scoreMultiplier = 1;
    private static float multiplierTimer = 0;
    private static float maxMultiplierTimer = 3;
    public static float timeTaken;
    public static float[] parTimes;
    public static int timeBonusEarned;
    public static bool victory = false;

    // Used for the results screen
    public static string lastSceneName;

    public static int[] rankScores = new int[] {0,0,0,0,0,0,0};

    public static string[] letterRanks = new string[] {"SS", "S","A","B","C","D","F"};

    public static void increaseScore(int increaseAmount)
    {
        score += Mathf.RoundToInt(increaseAmount * scoreMultiplier);
        //Update GUI
        if (GameObject.Find("scoreTextUpdate") != null)
        {
            GameObject.Find("scoreTextUpdate").GetComponent<Text>().text = "+ " + Mathf.RoundToInt(increaseAmount * scoreMultiplier);
        }
        if (scoreMultiplier < 5)
        {
            scoreMultiplier += 0.1f;
        }
        scoreMultiplier = Mathf.Round(scoreMultiplier * 10) / 10;
        multiplierTimer = 0;
    }

    public static void decreaseMultiplier()
    {
        multiplierTimer += Time.deltaTime;
        if (multiplierTimer >= maxMultiplierTimer)
        {
            scoreMultiplier = 1;
            multiplierTimer = 0;
            //Remove Score
            //Update GUI
            if (GameObject.Find("scoreTextUpdate") != null)
            {
                GameObject.Find("scoreTextUpdate").GetComponent<Text>().text = "";
            }
        }
    }

    public static string GetScoreRank()
    {
        for(int i = 0; i < rankScores.Length; i++)
        {
            if (score >= rankScores[i])
            {
                return letterRanks[i];
            }
            if (score <= 0)
            {
                return letterRanks[letterRanks.Length - 1];
            }
        }
        return "";
    }

    public static void CompareTimeToPar()
    {
        float tempTime = timeTaken;
        //Checks to see if there is a bonus
        if (tempTime <= parTimes[0] + parTimes[1])
        {
            //Took less time then par
            if(tempTime <= parTimes[0])
            {
                //FULL BONUS
                timeBonusEarned = (int)parTimes[2];
            }
            //Took more time then par
            else
            {
                tempTime = tempTime - parTimes[0];
                tempTime = tempTime / parTimes[1];
                tempTime -= 1;
                timeBonusEarned = (int)(parTimes[2] - ((tempTime * parTimes[2]) + parTimes[2]));
            }
        }
        else
        {
            //No bonus
            timeBonusEarned = 0;
        }
        score += timeBonusEarned;
    }
}



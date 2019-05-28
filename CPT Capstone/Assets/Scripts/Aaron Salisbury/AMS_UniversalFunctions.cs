using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AMS_UniversalFunctions
{
    public static int levelNumber;
    public static string missionLevel;

    //Mission text
    public static string[] objectivesText = {
        "1.Waves do not stop! Defeat the 3 Enemy Hives \n  \n2.Keep The Base Alive", //Hive Level
        "1.Survive 6 Waves \n \n2.Keep The Base Alive", //Wave Level
        "1.Defeat the HVT \n \n2.Keep The EVAC Alive", 
        "1.Find The Winning Resource \n \n2.Keep The Base Alive",
        "1.Defeat the 6 Assault Waves \n \n2.Keep The Base Alive", //Skyfall Level
        "1.Use the keys to open the locked doors. \n \n2.Escape to the elevator.",
        "1.Survive the 14 Wave Onslaught & Bring down as many opponents as possible.", //Arena Level
        "1.Follow all Instructions \n \n2.Finish Bootcamp" };

    public static void GoToResultsScreen(bool victory)
    {
        //Pass what level was just played
        AMS_ScoreController.victory = victory;
        AMS_ScoreController.lastSceneName = SceneManager.GetActiveScene().name;
        if (AMS_ScoreController.lastSceneName == "MissionBreifing")
        {
            AMS_ScoreController.lastSceneName = "Tutorial";
        }
        Cursor.visible = true;
        //Go to the Results Screen
        SceneManager.LoadScene("ResultsScreen");
    }
    public static void GoToMissionBreifing(int missionNumber, string levelAfter)
    {
        missionLevel = levelAfter;
        levelNumber = missionNumber;
        SceneManager.LoadScene("MissionBreifing2");
    }

    #region this is all dealing with finding median -KE
    //collapse above region to declutter
    private static int Partition<T>(this IList<T> list, int start, int end, System.Random rnd = null) where T : IComparable<T>
    {
        if (rnd != null)
            list.Swap(end, rnd.Next(start, end + 1));

        var pivot = list[end];
        var lastLow = start - 1;
        for (var i = start; i < end; i++)
        {
            if (list[i].CompareTo(pivot) <= 0)
                list.Swap(i, ++lastLow);
        }
        list.Swap(end, ++lastLow);
        return lastLow;
    }

    public static T NthOrderStatistic<T>(this IList<T> list, int n, System.Random rnd = null) where T : IComparable<T>
    {
        return NthOrderStatistic(list, n, 0, list.Count - 1, rnd);
    }
    private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, System.Random rnd) where T : IComparable<T>
    {
        while (true)
        {
            var pivotIndex = list.Partition(start, end, rnd);
            if (pivotIndex == n)
                return list[pivotIndex];

            if (n < pivotIndex)
                end = pivotIndex - 1;
            else
                start = pivotIndex + 1;
        }
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        if (i == j) return;
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    public static T Median<T>(this IList<T> list) where T : IComparable<T>
    {
        return list.NthOrderStatistic((list.Count - 1) / 2);
    }

    public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
    {
        var list = sequence.Select(getValue).ToList();
        var mid = (list.Count - 1) / 2;
        return list.NthOrderStatistic(mid);
    }

    #endregion
}

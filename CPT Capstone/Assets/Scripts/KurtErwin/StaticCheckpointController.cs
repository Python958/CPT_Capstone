using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticCheckpointController
{
    public static bool shouldTrigger = false;
    private static Vector3 locationToSpawn;
    public static bool[] checkPointList;

    public static void SetupHere(Vector3 place)
    {
        shouldTrigger = true;
        locationToSpawn = place;
        Debug.Log("set to setup");
    }

    public static void MovingToNewLevel()
    {
        shouldTrigger = false;
        var cpController = GameObject.FindObjectOfType<DLC_CheckPointController>();
        var array = cpController.manualCheckpoints;
        if (checkPointList == null) { checkPointList = new bool[array.Length]; }
        for (var i = 0; i < array.Length; i++)
        {
            checkPointList[i] = true;
        }
    }

    public static Vector3 WhereToSpawn()
    {
        return (locationToSpawn);
    }

    public static void SetCheckPointValue(DLC_Checkpoint checkPoint)
    {
        var cpController = GameObject.FindObjectOfType<DLC_CheckPointController>();
        var array = cpController.manualCheckpoints;
        if (array.Length != 0)
        {
            if (checkPointList == null)
            {
                checkPointList = new bool[array.Length];
                for(var i = 0; i < array.Length; i++) { checkPointList[i] = true; }
            }
            
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == checkPoint)
                {
                    checkPointList[i] = false;
                }
            }
        }
    }

    public static bool FindCheckPointValue(DLC_Checkpoint checkPoint)
    {
        if (checkPointList == null)
        {
            Debug.Log("didn't have a dictionary set up");
            return (true);
        }
        else
        {
            var cpController = GameObject.FindObjectOfType<DLC_CheckPointController>();
            var array = cpController.manualCheckpoints;
            if (array.Length != 0)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    if (array[i] == checkPoint)
                    {
                        if(checkPointList[i] == false)
                        { return (false); }
                        else { Debug.Log("Didn't have that value in the dictionary");  return (true); }
                    }
                }
                Debug.Log("Didn't find the component in the array");
                return (true);
            }
            else { Debug.Log("array length is zero");  return (true); }
        }
    }
}
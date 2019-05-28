using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class AMS_Hive : ParentHive
{

    public void LastHive()
    {
        foreach (Transform spawner in spawnLocations)
        {
            Destroy(spawner.gameObject);
        }
        if (GameObject.FindObjectsOfType<AMS_Hive>().Length == 1)
        {
            //Debug Player wins
            Debug.Log("Player destroyed all hives");
            AMS_UniversalFunctions.GoToResultsScreen(true);
        }
    }//cleans up and checks if this was the last hive

}

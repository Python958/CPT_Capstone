using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLC_CheckPointController : MonoBehaviour
{
    public GameObject[] checkpoints;
    public int checkpointIndex;
    public DLC_Checkpoint[] manualCheckpoints;

    private void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }

    void Update()
    {
       
    }

    public void UpdateCheckpoints(GameObject currentCheckpoint)
    {
        for(var i = 0; i < checkpoints.Length; i++)
        {
            if(checkpoints[i].GetComponent<DLC_Checkpoint>().status != DLC_Checkpoint.State.Inactive)
            {
                checkpoints[i].GetComponent<DLC_Checkpoint>().status = DLC_Checkpoint.State.Inactive;
            }
            if(checkpoints[i] == currentCheckpoint) { checkpointIndex = i; }
        }
        currentCheckpoint.GetComponent<DLC_Checkpoint>().status = DLC_Checkpoint.State.Active;
    }

    public GameObject FindNextCheckPoint()
    {
        var nextCheckpoint = checkpointIndex + 1;
        if(nextCheckpoint >= checkpoints.Length) { nextCheckpoint = 0; }

        return (checkpoints[nextCheckpoint]);
    }

    public bool IsCheckPointActive(GameObject checkThisCheckPoint)
    {
        for (var i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == checkThisCheckPoint)
            {
                var checkComponent = checkpoints[i].GetComponent<DLC_Checkpoint>();
                if (checkComponent != null)
                {
                    if (checkComponent.status == DLC_Checkpoint.State.Active) { return (true); }
                    else { return (false); }
                }
                else
                {
                    Debug.Log("can't find checkpoint component on object");
                    return (false);
                }
            }
        }
        Debug.Log("can't find the requested checkpoint");
        return (false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : ParentTurret
{
    protected override AMS_Health_Management AquireTarget()
    {
        var player = FindObjectOfType<KE_MainPlayer_Script>();
        if (player != null)
        {
            var playerObj = player.gameObject;
            float range = Vector3.Distance(playerObj.transform.position, transform.position);
            if (range <= turretRange)
            {
                var returnVal = playerObj.GetComponentInChildren<AMS_Health_Management>();
                if(returnVal == null) { Debug.Log("Can't find player health"); }
                return (returnVal);
            }
        }
        return (null);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyTurret : ParentTurret
{
    protected override void Start()
    {
        base.Start();
        dontHurtPlayer = true;
        bulletColor = Color.blue;
    }

    protected override AMS_Health_Management AquireTarget()
    {
        AMS_Health_Management returnValue = null;

        AMS_Health_Management[] enemyArray = FindObjectsOfType<AMS_Health_Management>();
        if (enemyArray.Length > 0)
        {
            float minTurretDis = turretRange;
            float minHiveDis = turretRange;
            AMS_Health_Management closestCont = null;
            AMS_Health_Management closestHive = null;
            foreach (AMS_Health_Management enemy in enemyArray)
            {
                if (enemy.tag == "Target" && enemy.currentHealth > 0 || enemy.tag == "Default_Enemy" && enemy.currentHealth > 0)
                {
                    var dis = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dis < minTurretDis)
                    {
                        minTurretDis = dis;
                        closestCont = enemy;
                    }
                }
                else if(enemy.gameObject.GetComponent<AMS_Hive>() != null)
                {
                    var dis = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dis < minHiveDis)
                    {
                        minHiveDis = dis;
                        closestHive = enemy;
                    }
                }
            }
            if (closestCont != null){ returnValue = closestCont; }
            else if(closestHive != null) { returnValue = closestHive; }

        }//there are enemies look to see if you hit

        return (returnValue);
    }
}

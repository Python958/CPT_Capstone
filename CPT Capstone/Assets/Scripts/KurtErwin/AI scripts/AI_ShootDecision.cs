using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Shoot")]
public class AI_ShootDecision : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        bool targetInRange = GetRange(controller);
        return (targetInRange);
    }

    public bool GetRange(DLC_StateController controller)
    {
        bool finalDecision = false;
        if (controller.chaseTarget != null)
        {
            var target = controller.chaseTarget.gameObject;
            var pointDistance = Vector3.Distance(target.transform.position, controller.eyes.position);
            var range = 10f;

            //if the enemy has an enemy gun use the ideal range on it
            var gun = controller.gameObject.GetComponent<EnemyGun>();
            if (gun != null) { range = gun.GetIdealRange(); } //this gets the varied range so they are not all stopping at the same point

            if (pointDistance < range)
            {
                finalDecision = true; //player is in range
            }
        }
        else
        {
            Debug.Log("can't find target in scene");
        }
        
        return (finalDecision);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/MeleeCheck")]
public class InMeleeRange : DLC_Decision
{

    public override bool Decide(DLC_StateController controller)
    {
        bool targetInRange = GetRange(controller);
        return (targetInRange);
    }
    public bool GetRange(DLC_StateController controller)
    {
        bool finalDecision = false;
        var target = controller.chaseTarget.gameObject;
        var playerX = controller.player.position.x;
        var playerZ = controller.player.position.z;
        var mRange = controller.meleeDistance;

        if (target != null)
        {
            var pointDistance = Vector3.Distance(target.transform.position, controller.eyes.position);
            var range = 10f;
            if (playerX >= mRange && playerZ >= mRange)
            {
                //if the enemy has an enemy gun use the ideal range on it
                var gun = controller.gameObject.GetComponent<EnemyGun>();
                if (gun != null) { range = gun.GetIdealRange(); } //this gets the varied range so they are not all stopping at the same point

                if (pointDistance < range)
                {
                    finalDecision = false; //player is NOT in melee range
                }               
            }
            if(playerX <= mRange && playerZ <= mRange)
            {
                finalDecision = true; //player is in melee range
            }
        }
        else
        {
            Debug.Log("can't find target in scene");
        }
        return (finalDecision);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/AttackBase")]
public class AI_AttackBase : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Home(controller);
    }

    private void Home(DLC_StateController controller)
    {
        var baseHealth = GameObject.FindObjectOfType<BaseHealth>();

        if (controller.enemyStats.tauntWorks)
        {
            var tauntTargets = GameObject.FindObjectsOfType<TauntStatue>();

            if (tauntTargets.Length > 0)
            {
                Transform validTauntTarget = null;
                float closestDist = Mathf.Infinity;

                for (var i = 0; i < tauntTargets.Length; i++)
                {
                    var dist = Vector3.Distance(tauntTargets[i].gameObject.transform.position, controller.eyes.position);
                    if (dist < closestDist && dist < tauntTargets[i].effectRange)
                    {
                        validTauntTarget = tauntTargets[i].gameObject.transform;
                    }
                }//go through all taunters and see if any is within range and select the closest

                if (validTauntTarget != null)
                {
                    Vector3 direction = controller.transform.position - validTauntTarget.position;
                    direction.y = 0f;
                    direction = direction.normalized;
                    direction *= 1.25f;

                    controller.navMeshAgent.destination = validTauntTarget.position + direction;
                    controller.navMeshAgent.speed = controller.enemyStats.walkSpeed;
                    //Debug.Log("There is a statue that is mocking me!");
                    controller.navMeshAgent.isStopped = false;

                    var pos = controller.transform.position + controller.navMeshAgent.velocity;
                    controller.gameObject.transform.LookAt(pos);
                    return;
                }//there is a valid taunt target so assign and then kill the rest of the function
            }//check for valid taunt targets
        }//only do taunt check if enemy can be effected by taunts

        if (baseHealth != null)
        {
            //this is so the enemy only checks every so often for the base
            //it doesn't move so it shouldn't need pathing every step
            if(Random.Range(0f, 1f) < .1f)
            {
                var baseTrans = baseHealth.gameObject.transform;

                //this offsets the goal so that it falls outside the cylinder
                Vector3 direction = controller.transform.position - baseTrans.position;
                direction.y = 0f;
                direction = direction.normalized;
                direction *= 4.5f; //this is the radius of the cylinder as set by the code in the cylinder

                controller.navMeshAgent.destination = baseTrans.position + direction;
                controller.navMeshAgent.speed = controller.enemyStats.walkSpeed;
                //Debug.Log("I'm homing on the base");
                controller.navMeshAgent.isStopped = false;

                var pos = controller.transform.position + controller.navMeshAgent.velocity;
                pos.y = controller.transform.position.y;
                controller.gameObject.transform.LookAt(pos);
            }
        }//there is a base so target it
        else
        {
            Debug.Log("there doesn't seem to be a base in this level!!");
        }
        
    }//homes in on taunt target or player base if it exists
}
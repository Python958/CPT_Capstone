using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PathToBase")]
public class AI_CanPathToBase : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        return(CheckPath(controller));
    }

    private bool CheckPath(DLC_StateController controller)
    {
        var baseHealth = GameObject.FindObjectOfType<BaseHealth>();
        if(baseHealth != null)
        {
            var baseTrans = baseHealth.gameObject.transform;
            NavMeshPath checkPath = new NavMeshPath();

            //this offsets the goal so that it falls outside the cylinder
            Vector3 direction = controller.transform.position - baseTrans.position;
            direction.y = 0f;
            direction = direction.normalized;
            direction *= 4.5f; //this is the radius of the cylinder as set by the code in the cylinder
            direction.y = -1f; // to move the final results a little closer to the nav mesh so it registers
            var goal = baseTrans.position + direction;

            Debug.DrawLine(controller.transform.position, goal);

            if (controller.navMeshAgent.CalculatePath(goal, checkPath))
            {
                if (checkPath.status == NavMeshPathStatus.PathPartial)
                {
                    //Debug.Log("path not complete");
                    return (false);
                }
                else
                {
                    //Debug.Log("path is complete");
                    return (true);
                }
            }
            else
            {
                //Debug.Log("couldn't find a path there");
                return (false);
            }
        }
        else
        {
            Debug.Log("there doesn't seem to be a base in this level!!");
            return (false); //obviously can't path to base
        }
    }//homes in on player base if it exists
}
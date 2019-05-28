using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Patrol")]
public class DLC_PatrolAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Patrol (controller);
    }

    private void Patrol (DLC_StateController controller)
    {
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position; //sets to zero
        controller.navMeshAgent.isStopped = false; //controller.navMeshAgent.Resume(); is obsolete
    
        
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending) //checking the distance from the next way point //Path Pending is a bool.
        {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count; //make sure not to exceed lenth of waypoint list; makes it loop back the waypoint
        }
    }
}
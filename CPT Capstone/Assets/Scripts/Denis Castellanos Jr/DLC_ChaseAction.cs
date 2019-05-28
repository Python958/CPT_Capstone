using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]

public class DLC_ChaseAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Chase(controller);
    }

    private void Chase(DLC_StateController controller)
    {
        if(controller.chaseTarget != null)
        {
            controller.navMeshAgent.destination = controller.chaseTarget.position;
            controller.navMeshAgent.speed = controller.enemyStats.runSpeed;
            // Debug.Log("I am chasing the player");
            controller.isSeen = true;

            var dist = Vector3.Distance(controller.eyes.transform.position, controller.chaseTarget.position);
            if (dist > 1.4) { controller.navMeshAgent.isStopped = false; }
            else { controller.navMeshAgent.isStopped = true; }

            var pos = controller.navMeshAgent.pathEndPosition;
            pos.y = controller.transform.position.y;
            controller.gameObject.transform.LookAt(pos);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RunAway")]
public class AI_LootGoblin :  DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Chase(controller);
    }

    private void Chase(DLC_StateController controller)
    {
       // controller.navMeshAgent.destination = controller.runAway.position;
        controller.navMeshAgent.speed = controller.enemyStats.runSpeed;
        Debug.Log("Run away little girl, Run away");
        controller.navMeshAgent.isStopped = false;

        var pos = controller.navMeshAgent.pathEndPosition;
        pos.y = controller.transform.position.y;
        controller.gameObject.transform.LookAt(pos);
    }
}


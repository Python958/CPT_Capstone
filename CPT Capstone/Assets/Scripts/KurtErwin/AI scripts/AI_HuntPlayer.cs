using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/HuntPlayer")]
public class AI_HuntPlayer: DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Hunt(controller);
    }

    private void Hunt(DLC_StateController controller)
    {
        var player = GameObject.FindObjectOfType<KE_MainPlayer_Script>();
        if(player != null)
        {
            controller.chaseTarget = player.transform;
            controller.navMeshAgent.destination = controller.chaseTarget.position;
            controller.navMeshAgent.speed = controller.enemyStats.walkSpeed;
            // Debug.Log("I am chasing the player");
            controller.navMeshAgent.isStopped = false;

            var pos = controller.navMeshAgent.pathEndPosition;
            pos.y = controller.transform.position.y;
            controller.gameObject.transform.LookAt(pos);
        }
        else
        {
            Debug.Log("there doesn't seem to be a player in this level!!");
        }
    }//homes in on player base if it exists
}
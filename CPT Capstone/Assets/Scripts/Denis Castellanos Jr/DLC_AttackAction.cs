using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Attack")]
public class DLC_AttackAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        
    }

    private void Attack(DLC_StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange)
        && hit.collider.CompareTag("Player"))
        {
            if(controller.CheckIfCountDownElapsed(controller.enemyStats.attackSpeed))
            {
                //controller.[enter attack script here].Fire(controller.enemyStats.attackforce, controller.enemyStats.attackspeed);
            }
        }
    }
}


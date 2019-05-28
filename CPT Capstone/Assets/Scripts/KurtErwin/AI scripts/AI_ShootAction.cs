using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Shoot")]
public class AI_ShootAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        Shoot(controller);
    }

    private void Shoot(DLC_StateController controller)
    {
        var gun = controller.gameObject.GetComponent<EnemyGun>();
        if(gun != null)
        {
            var lookPos = controller.chaseTarget.position;
            var charControl = controller.chaseTarget.gameObject.GetComponentInParent<CharacterController>();
            if(charControl != null)
            {
                var playerDir = charControl.velocity.normalized;
                var playerVelo = charControl.velocity.magnitude;
                var playerRange = Vector3.Distance(controller.chaseTarget.position, controller.gameObject.transform.position);
                var shotTime = playerRange / gun.bulletSpeed;
                var aimMag = playerVelo * shotTime;
                lookPos += playerDir * aimMag;
            }//this is to make the AI lead the player when moving;

            lookPos.y = controller.gameObject.transform.position.y;
            controller.gameObject.transform.LookAt(lookPos);
            gun.gunEnabled = true;
        }
        else { Debug.Log("can't find gun on enemy that needs to shoot"); }
    }
}


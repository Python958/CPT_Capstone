using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class AI_WanderAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Wander(controller);
    }

    private void Wander(DLC_StateController controller)
    {
        var distance = PathDistanceLeft(controller.navMeshAgent);

        if (distance < 1f) //(Random.Range(0f, 100f) < .1f)
        {
            float spread = 30f;
            Vector3 pos = controller.gameObject.transform.position;
            controller.navMeshAgent.speed = controller.enemyStats.walkSpeed;

            var vecOffset = SetVectorFromAngle(Random.Range(0f, 360f), Random.Range(spread / 2, spread));

            var newX = pos.x + vecOffset.x;
            var newY = pos.y;
            var newZ = pos.z + vecOffset.z;



            controller.navMeshAgent.destination = new Vector3(newX, newY, newZ);
            controller.navMeshAgent.isStopped = false;
        }
    }
 
    private Vector3 SetVectorFromAngle(float angle, float distance)
    {
        var rotation = Quaternion.AngleAxis(angle, Vector3.up);
        var forward = Vector3.forward * distance;
        return rotation * forward;
    }

    private float PathDistanceLeft(NavMeshAgent navAgent)
    {
        float distance = 0.0f;
        Vector3[] corners = navAgent.path.corners;
        for (int c = 0; c < corners.Length - 1; ++c)
        {
            distance += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
        }
        return (distance);
    }

}


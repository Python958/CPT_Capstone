using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Scan")]
public class DLC_ScanDecision : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(DLC_StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.transform.Rotate(0, controller.enemyStats.searchTurnSpeed * Time.deltaTime, 0);
        return controller.CheckIfCountDownElapsed(controller.enemyStats.searchLength);
    }
}
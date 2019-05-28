using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/ActiveState")]
public class DLC_ActiveStateDecision : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        bool chaseTargetIsActive = false;
        if(controller.chaseTarget != null)
        {
            chaseTargetIsActive = controller.chaseTarget.gameObject.activeSelf;
        }
        return chaseTargetIsActive;
    }
}

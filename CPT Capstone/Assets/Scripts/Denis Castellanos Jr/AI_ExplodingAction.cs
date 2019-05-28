using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Explode")]
public class AI_ExplodingAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        Explode(controller);
    }

    private void Explode(DLC_StateController controller)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/BossShieldCheck")]
public class AI_BossShieldCheck : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        bool returnValue = CheckForShield(controller);
        if(returnValue == false) { TurnOffBombTargeting(); }
        return (returnValue);
    }

    private bool CheckForShield(DLC_StateController controller)
    {
        var shield = FindObjectOfType<ForceField>();
        if(shield != null)
        {
            if(shield.HP > 0) { return (true); }
            else { Destroy(shield.gameObject); }
        }
        return (false);
    }

    private void TurnOffBombTargeting()
    {
        var bombScript = FindObjectOfType<RandomBomberScript>();
        if(bombScript != null) { bombScript.targetPlayer = false; }
    }
}
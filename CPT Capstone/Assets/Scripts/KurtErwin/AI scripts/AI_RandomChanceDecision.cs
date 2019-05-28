using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RandomChance")]
public class AI_RandomChanceDecision : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        float roll = Random.Range(0f, 1f);
        float chance = .3f;
        return (roll < chance ? true : false);
    }
}
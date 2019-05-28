using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HealthCheck")]
public class BossHealthCheck : DLC_Decision
{
    public override bool Decide(DLC_StateController controller)
    {
        var finalTrans = controller.eyes.gameObject.transform;
        while (finalTrans.parent != null) { finalTrans = finalTrans.parent; }
        var health = finalTrans.gameObject.GetComponentInChildren<AMS_Health_Management>();
        if (health != null)
        {
            if (!controller.enemyStats.usedShield)
            {
                if (health.currentHealth < health.maxHealth / 2f)
                {
                    Debug.Log("should be activating shield!!!!!!!!!!!!!!!!!!");
                    controller.enemyStats.usedShield = true;
                    return (true);
                }
                else { Debug.Log("health not below 50perc");
                    return (false); }
            }
            else { Debug.Log("already used shield");
                return (false); }
        }
        else
        {
            Debug.Log("can't find health component!!!!!!!!!!!!!");
            return (false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/BombPlayer")]
public class AI_BossBombPlayerAction : DLC_Action
{
    public override void Act(DLC_StateController controller)
    {
        BombPlayer(controller);
    }

    private void BombPlayer(DLC_StateController controller)
    {
        Debug.Log("bombing Player");

        controller.navMeshAgent.isStopped = true;

        var bombingScript = FindObjectOfType<RandomBomberScript>();
        if(bombingScript != null) { bombingScript.targetPlayer = true; }

        var shield = FindObjectOfType<ForceField>();
        if(shield == null){ CreateShield(controller); }
        else { shield.transform.position = controller.eyes.transform.position; }

        //with random chance you could spawn enemy!!!
    }

    private void CreateShield(DLC_StateController controller)
    {
        Debug.Log("created Shield");
        var shield = Instantiate(controller.enemyStats.ForceField, controller.eyes.transform.position, controller.eyes.transform.rotation);
        var field = shield.GetComponent<ForceField>();
        field.owner = controller.eyes.gameObject;
    }
}


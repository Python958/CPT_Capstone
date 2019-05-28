using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class DLC_LookDecision : DLC_Decision
{
    private float maxAwarenessDistance = 2f;

    public override bool Decide(DLC_StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(DLC_StateController controller)
    {
        //Debug.DrawRay(controller.eyes.position, controller.eyes.forward * 2f, Color.red);

        //check if any other enemies are alerted but do it less often if already alerted
        var baseScript = controller.enemyStats.baseScript;

        if (baseScript == null && !controller.enemyStats.checkedForBase)
        {
            baseScript = FindObjectOfType<BaseHealth>();
            controller.enemyStats.baseScript = baseScript;
            controller.enemyStats.checkedForBase = true;
        }//should only have to do once
        
        GameObject baseObject = null;
        if(baseScript != null) { baseObject = baseScript.gameObject; }

        if(Random.Range(1,100) < 25)
        {
            if (controller.chaseTarget == null
            || (baseObject != null && controller.chaseTarget == baseObject.transform))
            {
                var rallyTarget = RallyTroops(controller);
                if (rallyTarget != null) { controller.chaseTarget = rallyTarget; return (true); }
            }
        }//lower the number of calls to rally troops to about every four steps

        if(controller.chaseTarget != null
            && (controller.chaseTarget.gameObject.name.Contains("TauntStatue") || Random.Range(1, 100) < 20))
        {
            var taunts = FindObjectsOfType<TauntStatue>();
            List<GameObject> visibleTaunts = new List<GameObject>();
            if (taunts.Length > 0)
            {
                foreach (TauntStatue statue in taunts)
                {
                    if (CheckView(statue.gameObject, controller.eyes.gameObject, controller.enemyStats.lookDistance, controller.enemyStats.lookConeOfView)) { visibleTaunts.Add(statue.gameObject); }
                }//find visible statues

                if (visibleTaunts.Count > 0)
                {
                    float closest = Mathf.Infinity;
                    GameObject targetStatue = null;
                    foreach (GameObject statue in visibleTaunts)
                    {
                        float dis = Vector3.Distance(statue.transform.position, controller.eyes.position);
                        if (dis < closest)
                        {
                            closest = dis;
                            targetStatue = statue;
                        }
                    }
                    controller.chaseTarget = targetStatue.transform;
                    return (true);
                }//find closest of visible
            }//check taunt statues
        }//lower the number of checks for statues but always check if chasing a statue

        var playerScript = controller.enemyStats.playerScript;

        if(playerScript == null)
        {
            playerScript = FindObjectOfType<KE_MainPlayer_Script>();
            controller.enemyStats.playerScript = playerScript;
        }//should only be called once

        if (playerScript != null)
        {
            var player = playerScript.gameObject;

            if (CheckView(player, controller.eyes.gameObject, controller.enemyStats.lookDistance, controller.enemyStats.lookConeOfView))
            {
                controller.chaseTarget = player.transform;
                return (true);
            }
            return (false);
            
        }

        Debug.Log("could not find player object");
        return (false);
    }

    private Transform RallyTroops(DLC_StateController controller)
    {
        var troops = FindObjectsOfType<DLC_StateController>();
        foreach(DLC_StateController troop in troops)
        {
            var parentTrans = troop.chaseTarget;

            if (parentTrans != null)
            {
                while (parentTrans.parent != null) { parentTrans = parentTrans.parent; }
                var parentObj = parentTrans.gameObject;

                if (parentObj.tag == "Player" || parentObj.tag == "Taunt")
                {
                    var dis = Vector3.Distance(troop.gameObject.transform.position, controller.eyes.position);
                    if(dis < controller.enemyStats.lookDistance/2)
                    {
                        return (troop.chaseTarget);
                    }
                }
            }
        }
        return (null);
    }

    private bool CheckView(GameObject target, GameObject looker, float maxView, float coneOfView)
    {
        //check if player is located
        var directionToTarget = target.transform.position - looker.transform.position;
        var distanceToTarget = directionToTarget.magnitude; //get the distance to player

        if (distanceToTarget < maxView)
        {
            directionToTarget = Vector3.Normalize(directionToTarget);
            var angleOffCenter = Vector3.Angle(looker.transform.forward, directionToTarget); //find the angle off center the player is
            
            //controller.isSeen = true;

            var currentViewAngle = coneOfView * ((maxView - distanceToTarget) / maxView);   //this is what lowers the field of view as the player gets farther away
            currentViewAngle = Mathf.Max(3f, currentViewAngle);                             //maintains a small field of view no matter how far player gets away

            //deal with drawing view cone
            bool isStealthLevel = false;
            if (isStealthLevel)
            {
                var eyeObj = looker;
                LineRenderer lineRenderer = eyeObj.GetComponent<LineRenderer>();

                if (lineRenderer == null)
                {
                    lineRenderer = eyeObj.AddComponent<LineRenderer>();
                    lineRenderer.receiveShadows = false;
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.loop = false;
                    lineRenderer.positionCount = 3;
                }//set up the line renderer if it hasn't been already

                Vector3[] triangle = new Vector3[3];
                {
                    Ray ray = new Ray();
                    ray.origin = eyeObj.transform.position;

                    triangle[1] = eyeObj.transform.position;

                    ray.direction = Quaternion.Euler(0f, -currentViewAngle, 0f) * looker.transform.forward;
                    triangle[0] = ray.GetPoint(maxView);
                    ray.direction = Quaternion.Euler(0f, currentViewAngle, 0f) * looker.transform.forward;
                    triangle[2] = ray.GetPoint(maxView);
                }//here we assign all the points to the triangle of the view

                lineRenderer.SetPositions(triangle);
            }//draws something in game view if this is the stealth level
            else
            {
                Debug.DrawRay(looker.transform.position, Quaternion.Euler(0f, -currentViewAngle, 0f) * looker.transform.forward * maxView, Color.blue);
                Debug.DrawRay(looker.transform.position, Quaternion.Euler(0f, currentViewAngle, 0f) * looker.transform.forward * maxView, Color.blue);
            }//not stealth level so only draw in debug views

            if (angleOffCenter < currentViewAngle || distanceToTarget < maxAwarenessDistance)
            {
                if (distanceToTarget < maxAwarenessDistance)
                {
                    return (true);
                }
                else
                {
                    RaycastHit hit;
                    var layer = (1 << 0) | (1 << 9);
                    Debug.DrawRay(looker.transform.position, directionToTarget * distanceToTarget, Color.blue);
                    if (Physics.Raycast(looker.transform.position, directionToTarget, out hit, maxView, layer, QueryTriggerInteraction.Ignore))
                    {
                        var parentTrans = hit.transform;
                        while (parentTrans.parent != null) { parentTrans = parentTrans.parent; }
                        var parentObj = parentTrans.gameObject;
                        if(parentObj == target)
                        {
                            return (true);
                        }
                        else
                        {
                            //Debug.Log("raycast did not hit target");
                            //Debug.Log(hit.transform.gameObject);
                            return (false);
                        }
                    }
                    else
                    {
                        //Debug.Log("raycast hit nothing");
                        return (false);
                    }
                }//check if they have LOS to target

            }//the target is within the cone of view
            else
            {
                //controller.isSeen = false;
                //Debug.Log("target is outside of peripheral");
                return (false);
            }//target is outside of cone of view
        }//within view range now check if visible

        //Debug.Log("target is outside of view range");
        return (false);
    }
}

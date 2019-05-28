using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointerScript : MonoBehaviour
{
    public Vector3 sizeConstraints;
    private Transform playerTrans;
    public Transform enemyTrans;
    private SpriteRenderer arrow;
    private AMS_Health_Management healthScript;
    
    // Start is called before the first frame update
    void Start()
    {
        arrow = gameObject.GetComponent<SpriteRenderer>();

        if (enemyTrans == null) { Debug.Log("enemy transform not assigned"); }
        else
        {
            var healthDestroy = gameObject.AddComponent<GenericHealthDestroy>();
            healthDestroy.targetObj = enemyTrans.gameObject;
        }
        var playerScript = FindObjectOfType<KE_MainPlayer_Script>();
        if(playerScript != null)
        {
            playerTrans = playerScript.gameObject.transform;
            var cam = playerScript.gameObject.GetComponentInChildren<Camera>();
            if(cam != null)
            {
                sizeConstraints.x = (cam.orthographicSize) * .95f;
                sizeConstraints.z = (sizeConstraints.x * cam.aspect);
            }
            else { Debug.Log("can't find minimap camera"); }
        }
        else { Debug.Log("can't find a player in the scene"); }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyTrans != null && playerTrans != null)
        {
            Vector3 ray = enemyTrans.position - playerTrans.position;

            var horizPerc = sizeConstraints.x / Mathf.Abs(ray.x);
            var vertPerc = sizeConstraints.z / Mathf.Abs(ray.z);

            if(horizPerc < 1f || vertPerc < 1f)
            {
                if (!arrow.enabled) { arrow.enabled = true; }
                
                //use the lowest percent
                var percentToUse = (horizPerc < vertPerc) ? horizPerc : vertPerc;

                Vector3 clampedOffset = new Vector3(ray.x * percentToUse, 0f, ray.z * percentToUse);

                var newPos = playerTrans.position + clampedOffset;
                newPos.y = sizeConstraints.y;

                gameObject.transform.position = newPos;
            }//make sure the enemy is off screen
            else
            {
                //Debug.Log("arrow out of ZONE");
                if (arrow.enabled) { arrow.enabled = false; }
            }//on screen hide arrow

        }
        else
        {
            //Debug.Log("lost the enemy or player");
            Destroy(gameObject);
        }//happens usually when enemy dies (or player)
    }
}

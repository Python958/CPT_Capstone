using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetize : MonoBehaviour
{

    private Transform playerTrans = null;
    public float attractRange;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTrans == null) { FindPlayer(); }
        else
        {
            var dis = Vector3.Distance(transform.position, playerTrans.position);
            if(dis <= attractRange)
            {
                var dir = playerTrans.position - transform.position;
                dir = dir.normalized;
                var moveSpeedMod = AI_StaticFunctions.LogisticFunction((attractRange - dis) / attractRange);
                dir *= moveSpeedMod * moveSpeed;

                transform.position += dir * Time.deltaTime;
            }//player is close enough to effect resource
        }//apply attraction
    }

    private void FindPlayer()
    {
        var playerScript = FindObjectOfType<KE_MainPlayer_Script>();
        if (playerScript == null) { Debug.Log("Can't find the main player script"); }
        else { playerTrans = playerScript.transform; }
    }//because we don't want to lose them do we?
}

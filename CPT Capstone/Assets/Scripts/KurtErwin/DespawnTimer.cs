using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    public float timerStart;
    private float timerCurrent;
    public bool killParent = true;

    //for the blinking
    public float alertShowTime = .5f;
    public float alertOffTime = .05f;
    private float lastTime;
    private bool blinkFlag = false;
        
    // Start is called before the first frame update
    void Start()
    {
        timerCurrent = timerStart;
        lastTime = timerStart;

        alertOffTime = alertOffTime > 0 ? alertOffTime : .05f;
        alertShowTime = alertShowTime > 0 ? alertShowTime : .5f;
    }

    // Update is called once per frame
    void Update()
    {
        timerCurrent -= Time.deltaTime;
        OperateDespawnAlert();
        if (timerCurrent <= 0f)
        {
            var destroyObject = gameObject;

            if (killParent)
            {
                Transform targetParent = gameObject.transform.parent;

                while (targetParent != null)
                {
                    destroyObject = targetParent.gameObject;
                    targetParent = targetParent.parent;
                }
            }//go up the inheritance chain to find final parent

            Destroy(destroyObject);
        }
    }

    private void OperateDespawnAlert()
    {
        var timerPercent = timerCurrent / timerStart;
        if (timerPercent <= .35f)
        {
            var t = lastTime - timerCurrent;
            var targetTime = blinkFlag ? alertShowTime : alertOffTime;

            if (t >= targetTime)
            {
                lastTime = timerCurrent;
                MeshRenderer[] rends = GetComponentsInChildren<MeshRenderer>();
                if(rends.Length > 0)
                {
                    foreach(MeshRenderer rend in rends)
                    {
                        rend.enabled = blinkFlag;
                    }
                }
                blinkFlag = !blinkFlag;
            }
        }
    }
}

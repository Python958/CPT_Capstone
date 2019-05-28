using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawnScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject ownedBarrel;
    public GameObject resObject;
    public float barrelRespawnRate; //number of seconds before resource respawns
    private float barrelTimer = 0f;
    private bool alarmSet = true;

    // Update is called once per frame
    void Update()
    {
        TrySpawnBarrel();
    }

    public void TrySpawnBarrel()
    {
        if (barrelTimer <= 0f)
        {
            if (ownedBarrel == null)
            {
                if (!alarmSet)
                {
                    barrelTimer = barrelRespawnRate;
                    alarmSet = true;
                }
                else
                {
                    ownedBarrel = Instantiate(resObject, gameObject.transform.position, gameObject.transform.rotation);
                    var dragScript = ownedBarrel.GetComponent<DragScript>();
                    dragScript.grounded = false;
                    alarmSet = false;
                }
            }
        }
        else { barrelTimer -= Time.deltaTime; }
    }
}

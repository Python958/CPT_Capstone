using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AMS_TarPit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.GetComponentInParent<DLC_StateController>() != null)
            {
                other.GetComponentInParent<DLC_StateController>().enemyStats.walkSpeed = other.GetComponentInParent<DLC_StateController>().enemyStats.walkSpeed / 2;
                other.GetComponentInParent<DLC_StateController>().enemyStats.runSpeed = other.GetComponentInParent<DLC_StateController>().enemyStats.runSpeed / 2;
                Debug.Log("Has Stats");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<DLC_StateController>() != null)
        {
            other.GetComponentInParent<DLC_StateController>().enemyStats.walkSpeed = other.GetComponentInParent<DLC_StateController>().enemyStats.walkSpeed * 2;
            other.GetComponentInParent<DLC_StateController>().enemyStats.runSpeed = other.GetComponentInParent<DLC_StateController>().enemyStats.runSpeed * 2;
            Debug.Log("Has Stats");
        }
    }
}

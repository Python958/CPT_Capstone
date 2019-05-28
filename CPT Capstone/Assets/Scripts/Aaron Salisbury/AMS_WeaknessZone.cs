using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_WeaknessZone : MonoBehaviour
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
            if (other.GetComponent<AMS_Health_Management>())
            {
                AMS_Health_Management manager = other.GetComponent<AMS_Health_Management>();
                if(manager.healthWeakend == 0)
                {
                    manager.maxHealth = manager.maxHealth / 2;
                    manager.currentHealth = manager.currentHealth / 2;
                }
                manager.healthWeakend++;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.GetComponent<AMS_Health_Management>())
            {
                AMS_Health_Management manager = other.GetComponent<AMS_Health_Management>();
                manager.healthWeakend--;
                if (manager.healthWeakend == 0)
                {
                    manager.maxHealth = manager.maxHealth * 2;
                    manager.currentHealth = manager.currentHealth * 2;
                }
            }
        }
    }
}

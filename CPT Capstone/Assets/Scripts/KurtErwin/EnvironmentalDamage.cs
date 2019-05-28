using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalDamage : MonoBehaviour
{

    public int damageAmount;
    public bool ignoreInvincible;
    public bool targetEveryone;
    public float duration = 0;

    private void OnTriggerStay(Collider other)
    {
        AMS_Health_Management health = null;

        if (!targetEveryone)
        {
            if (other.CompareTag("Player"))
            {
                health = other.gameObject.GetComponent<AMS_Health_Management>();
                DamageSomeone(health);
            }
            else
            {
                //Debug.Log("something other than player in trigger");
            }
        }
        else
        {
            health = other.gameObject.GetComponent<AMS_Health_Management>();
            if(health == null) { health = other.gameObject.GetComponentInChildren<AMS_Health_Management>(); }
            if(health == null) { health = other.gameObject.GetComponentInParent<AMS_Health_Management>(); }
            DamageSomeone(health);
        }
    }

    private void Update()
    {
        if (duration != 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DamageSomeone(AMS_Health_Management health)
    {
        if (health != null)
        {
            if (ignoreInvincible)
            {
                health.TakeDamageUnfair(damageAmount);
            }
            else
            {
                health.TakeDamage(damageAmount, "environment");
            }
        }
        else
        {
            //Debug.Log("cannot find health component where it is expected!!");
        }
    }
}

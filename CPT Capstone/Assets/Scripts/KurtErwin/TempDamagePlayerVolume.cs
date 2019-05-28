using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDamagePlayerVolume : MonoBehaviour
{
    public int damageAmount;
    private AMS_Health_Management health;
    public float coolDownMax;                   //how often the enemy can hit you
    private float currentCoolDown;
    private BaseHealth baseHealth;
    private Collider lastCollided;
    private AMS_Health_Management lastHealth;

    private void Start()
    {
        health = GetComponentInParent<AMS_Health_Management>();
        currentCoolDown = 0f;

        baseHealth = FindObjectOfType<BaseHealth>();
    }

    private void Update()
    {
        if (health != null)
        {
            if (health.currentHealth <= 0) { Destroy(gameObject); }
        }
        if (currentCoolDown > 0f) { currentCoolDown -= Time.deltaTime; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentCoolDown <= 0f)
        {
            if (other.transform.parent != null && other.transform.parent.CompareTag("Taunt") || other.gameObject.CompareTag("Player"))
            {
                AMS_Health_Management victimHealth;

                if (other == lastCollided)
                {
                    victimHealth = lastHealth;
                }
                else
                {
                    lastCollided = other;
                    if (other.gameObject.CompareTag("Player"))
                    {
                        victimHealth = other.GetComponent<AMS_Health_Management>();
                    }
                    else
                    {
                        victimHealth = other.transform.parent.GetComponentInChildren<AMS_Health_Management>();
                    }
                    lastHealth = victimHealth;
                }

                if (victimHealth != null)
                {
                    victimHealth.TakeDamage(damageAmount, "enemy volume");
                    currentCoolDown = coolDownMax;
                }
                else { Debug.Log("can't find health component"); }
            }//damage the player
            else if (baseHealth != null && baseHealth.invulnerable == false)
            {
                if(other.name == "BaseHealth")
                {
                    baseHealth.currentHP -= damageAmount;
                    currentCoolDown = coolDownMax;
                }
            }
        }
    }
}

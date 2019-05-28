using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BaseProjectile
{
    Vector3 m_direction;
    bool m_fired;
    public GameObject m_launcher;
    GameObject m_target;
    int m_damage;
    public float maxDistance = 50;
    private float currentDistance = 0;

    // Update is called once per frame
    void Update()
    {
        if (m_fired)
        {
            transform.position += transform.forward * Time.deltaTime * 30;
            currentDistance += 1 * Time.deltaTime;
            if (currentDistance > maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
    //Overwrites the Base projectile Abstract class to fire the projectile with the values given from TurretShooting
    public override void FireProjectile(GameObject launcher, GameObject target, int damage, float attackSpeed)
    {
        if (launcher && target)
        {
            m_direction = (target.transform.position + launcher.transform.position).normalized;
            m_fired = true;
            m_launcher = launcher;
            m_target = target;
            m_damage = damage;

            Destroy(gameObject, 10.0f);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        //If the object has a health system deal damage
        if (col.gameObject.GetComponent<AMS_Health_Management>())
        {
            //"Player" shows the player dealt the damage.
            col.gameObject.GetComponent<AMS_Health_Management>().TakeDamage(m_damage, "Player");
        }
        //Ignore List
        if (col.gameObject.tag != "Turret" && col.gameObject.tag != "Mine" && col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    } 
}


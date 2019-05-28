using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_Bullet : MonoBehaviour
{
    public int contactDamage;
    public string type;
    public float spread = 4;
    public float maxDistance = 50;
    public float speed;
    private float currentDistance = 0;
    public bool lockToY = true;
    public bool dontDamagePlayer = false;
    public bool dontDamageEnemy = false;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        if (lockToY) { transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + Random.Range(-spread, spread), 0); ; }// transform.eulerAngles.z + Random.Range(-spread, spread)); }
        else{ transform.eulerAngles = new Vector3(transform.eulerAngles.x + Random.Range(-spread, spread), transform.eulerAngles.y + Random.Range(-spread, spread), transform.eulerAngles.z + Random.Range(-spread, spread)); }
        //Bullet Spread
    }

    // Update is called once per frame
    void Update()
    {
        //Move forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        currentDistance += 1 * Time.deltaTime;
        if (currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }
        if (type == "Ricochet")
        {
            Bounce();
        }
        if (type == "Shotgun")
        {
            Shrink();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //If the object has a health system deal damage
        if (col.gameObject.GetComponent<AMS_Health_Management>())
        {
            //"Player" shows the player dealt the damage.
            var parentTrans = col.gameObject.transform;
            while (parentTrans.parent != null) { parentTrans = parentTrans.parent; }
            var parentObj = parentTrans.gameObject;
            if (dontDamagePlayer)
            {
                if(parentObj.GetComponentInChildren<KE_MainPlayer_Script>() == null) { col.gameObject.GetComponent<AMS_Health_Management>().TakeDamage(contactDamage, type); }
            }
            else if (dontDamageEnemy)
            {
                if (parentObj.tag != "Default_Enemy") { col.gameObject.GetComponent<AMS_Health_Management>().TakeDamage(contactDamage, type); }
            }
            else { col.gameObject.GetComponent<AMS_Health_Management>().TakeDamage(contactDamage, type); } //not worried about who we are damaging
             //Destory on barrels
            if (parentObj.GetComponentInChildren<ExplosiveBarell>() != null)
            {
                //except rail gun
                if (type != "Rail Gun")
                {
                    Destroy(gameObject);
                }
            }
        }
        //Ignore List
        if(col.gameObject.tag != "Turret"
            && col.gameObject.tag != "Mine"
            && col.gameObject.tag != "Resource"
            && col.gameObject.tag != "Base"
            && col.gameObject.tag != "Eyes"
            && col.gameObject.tag != "Hazard"
            && col.gameObject.layer != 2
            && col.gameObject.GetComponent<AMS_Bullet>() == null
            && col.gameObject.GetComponent<DLC_Checkpoint>() == null
            && col.gameObject.GetComponent<EnvironmentalDamage>() == null
            && col.gameObject.GetComponent<InstantResourceScript>() == null
            && col.gameObject.GetComponent<AMS_PowerupPickup>() == null)
        {
            var shield = col.gameObject.GetComponent<ForceField>();
            if(shield != null)
            {
                shield.HP--;
            }

            if (type != "Rail Gun")
            {
                if (type == "Ricochet"
                    && col.gameObject.tag != "Default_Enemy"
                    && col.gameObject.tag != "Enemy"
                    && col.gameObject.tag != "Player"
                    && col.gameObject.tag != "Eyes")
                {
                    //Bounce();
                }
                else
                {
                    
                    Destroy(gameObject);
                }
            }
            //Dont destroy after touching an enemy
            else if (col.gameObject.tag != "Default_Enemy" && col.gameObject.tag != "Enemy" && col.gameObject.tag != "Target")
            {
                Destroy(gameObject);
            }
        }
    }

    private void Bounce()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 1))
        {
            //Dont bounce
            if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Default_Enemy" && hit.collider.gameObject.tag != "Hazard" && hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "Target")
            {
                Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, rot, 0);
            }
        }
    }

    private void Shrink()
    {
        var bulletScale = transform.localScale.x;
        bulletScale = Mathf.Max(maxDistance - currentDistance, 0f);
        time -= Time.deltaTime;
        transform.localScale = new Vector3(0.3f, bulletScale, bulletScale);
    }
}

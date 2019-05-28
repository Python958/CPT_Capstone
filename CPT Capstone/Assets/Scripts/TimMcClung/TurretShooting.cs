using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretShooting : MonoBehaviour
{
    public float fireRate;
    public int damage;
    public float fieldOfView;
    public bool beam;
    public GameObject projectile;
    public GameObject projectileSpawns;
   
    private int currentTurretAmmo; //DLC - To check current turret ammo
    public int maxTurretAmmo = 50; //DLC - Max Ammo
    public Text turretAmmoText; //Displays ammo text on the HUD
    //private bool TurretIsShooting = false;

    List<GameObject> m_lastProjectiles = new List<GameObject>();
    float m_fireTimer = 0.0f;
    GameObject m_target;

    private void Start()
    {
        currentTurretAmmo = maxTurretAmmo;
        
    }

    // Update is called once per frame
    void Update()
    {
        m_fireTimer += Time.deltaTime;

        if (m_fireTimer >= fireRate)
        {
            if (m_target != null)
            {
                float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(m_target.transform.position - transform.position));

                if (angle < fieldOfView)
                {
                    SpawnProjectiles();

                    m_fireTimer = 0.0f;
                }
            }
        }

    }

    void SpawnProjectiles()
    {
        if (!projectile)
        {
            return;
        }

        m_lastProjectiles.Clear();

        //for (int i = 0; i < projectileSpawns.Count; i++)
        {

            if (projectileSpawns)
            {
                GameObject proj = Instantiate(projectile, projectileSpawns.transform.position, Quaternion.Euler(projectileSpawns.transform.forward)) as GameObject;
                proj.GetComponent<BaseProjectile>().FireProjectile(projectileSpawns, m_target, damage, fireRate);
                proj.transform.eulerAngles = projectileSpawns.transform.eulerAngles;
                m_lastProjectiles.Add(proj);

                {
                    //TurretIsShooting = true;
                    currentTurretAmmo--;
                    Debug.Log("Ammo Left " + currentTurretAmmo);
                    turretAmmoText.text = "" + currentTurretAmmo.ToString();
                    
                    if (currentTurretAmmo <= 0)
                    {
                        fireRate = 10000;
                        Debug.Log("The Turrent has no more ammo and self destructed");
                        Destroy(gameObject);
                    }
                }
                

            }
        }
    }

    public void SetTarget(GameObject target)
    {
        m_target = target;
    }

    void RemoveLastProjectiles()
    {
        while (m_lastProjectiles.Count > 0)
        {
            Destroy(m_lastProjectiles[0]);
            m_lastProjectiles.RemoveAt(0);
        }
    }
}


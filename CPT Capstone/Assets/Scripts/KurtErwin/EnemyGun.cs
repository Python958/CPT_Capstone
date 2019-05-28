using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject currentBullet;
    public GameObject spawnLocation;
    public float maxFireRate = .1f;
    private float currentFireRate;
    public bool gunEnabled = false;
    public AudioClip defaultSound;
    public float idealRange;
    private float currentIdealRange;
    private float rangeTimer = 0f;
    public float rangeTimerMax;
    private readonly float rangeSpread = .5f;
    public float bulletSpeed;
    public float bulletSpread;
    public float bulletTimer;
    public int bulletDamage;

    private void Start()
    {
        ModifyIdealRange();

        //check for editor input or go with default values
        bulletSpeed = (bulletSpeed > 0f) ? bulletSpeed : 10f;
        bulletTimer = (bulletTimer > 0f) ? bulletTimer : 3f;
        bulletDamage = (bulletDamage > 0) ? bulletDamage : 10;
    }


    void Update()
    {
        if (gunEnabled) //gun enabled means it is shooting
        {
            if (currentFireRate > maxFireRate)
            {
                currentFireRate = 0;
                ShootBullet();
                rangeTimer = 0f;
            }
            else { currentFireRate += Time.deltaTime; } //"if" so it doesn't count up indefinitely
        }

        //this is to allow it to occasionally change it's ideal range
        if (rangeTimer < rangeTimerMax)
        {
            if (rangeTimer != -1f) { rangeTimer += Time.deltaTime; }
        }
        else
        {
            ModifyIdealRange();
            rangeTimer = -1f;
        }

        gunEnabled = false;
    }

    private void ModifyIdealRange()
    {
        var spread = idealRange * rangeSpread;
        currentIdealRange = (idealRange - (spread / 2f)) + Random.Range(0f, spread);
        currentFireRate = maxFireRate - Random.Range(0, 1);
    }//this randomizes the ideal range and the time before first shots next

    private void ShootBullet()
    {
        GameObject Tempbullet = Instantiate(currentBullet, new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y, spawnLocation.transform.position.z), Quaternion.identity);
        Tempbullet.transform.eulerAngles = transform.eulerAngles;
        var bulletScript = Tempbullet.GetComponent<AMS_Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
            bulletScript.spread = bulletSpread;
            bulletScript.maxDistance = bulletTimer;
            bulletScript.contactDamage = bulletDamage;
        }
        else { Debug.Log("can't find bullet script to modify"); }

        var trail = Tempbullet.GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.endColor = Color.red;
            trail.startColor = Color.red;
        }
        else { Debug.Log("bullet does not have a trail renderer"); }
    }

    public float GetIdealRange() { return (currentIdealRange); }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentTurret : MonoBehaviour
{
    public float turretRange;
    public float turretBaseRotationSpeed;
    public float turretCurrentRotationSpeed;
    protected float deadZone = 10f;

    public GameObject currentBullet;
    public Transform spawnLocation;
    public float shotBaseTimerMax;
    public float shotCurrentTimerMax;
    protected float shotTimerCurrent;
    public AudioClip defaultSound;
    public float bulletSpeed;
    public float bulletSpread;
    public float bulletTimer;
    public int bulletDamage;
    public int ammoMax;
    protected int ammoCurrent;
    public float reloadMax;
    protected float reloadCurrent;
    public bool usesAmmo = true;
    protected bool dontHurtPlayer = false;
    protected Color bulletColor = Color.red;

    private AudioSource turretAudio;

    private GameObject particleObject;
    public float partTimer;

    [HideInInspector]
    public float turretRotationBuff = 0f;
    [HideInInspector]
    public bool leading = false;
    [HideInInspector]
    public bool cheatLeading = false;
    [HideInInspector]
    public bool offline = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            turretAudio = GetComponent<AudioSource>();
        }
        var parts = GetComponentInChildren<ParticleSystem>();
        if(parts != null)
        {
            particleObject = parts.gameObject;
            SwitchParticles(false);
        }
        else { particleObject = null; }

        shotCurrentTimerMax = shotBaseTimerMax;
        shotTimerCurrent = shotCurrentTimerMax;

        //check for editor input or go with default values
        turretRange = (turretRange > 0f) ? turretRange : 50f;
        turretBaseRotationSpeed = (turretBaseRotationSpeed > 0f) ? turretBaseRotationSpeed : 120f;
        turretCurrentRotationSpeed = turretBaseRotationSpeed;
        shotBaseTimerMax = (shotBaseTimerMax > 0f) ? shotBaseTimerMax : .2f;
        shotCurrentTimerMax = shotBaseTimerMax;

        bulletSpeed = (bulletSpeed > 0f) ? bulletSpeed : 18f;
        bulletTimer = (bulletTimer > 0f) ? bulletTimer : 3f;
        bulletDamage = (bulletDamage > 0) ? bulletDamage : 10;

        ammoMax = (ammoMax > 0) ? ammoMax : 8;
        ammoCurrent = ammoMax;
        reloadMax = (reloadMax > 0f) ? reloadMax : 3;
        reloadCurrent = reloadMax;

        if (currentBullet == null) { Debug.Log("assign default bullet to current bullet"); }
        if (spawnLocation == null) { spawnLocation = transform; }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!offline)
        {
            if (reloadCurrent > 0f && ammoCurrent == 0)
            {
                reloadCurrent -= Time.deltaTime;
                if (reloadCurrent <= 0f) { reloadCurrent = reloadMax; ammoCurrent = ammoMax; }
            }
            var target = AquireTarget();
            if (target != null)
            {
                var dir = target.transform.position - transform.position;
                Debug.DrawRay(transform.position, dir, Color.blue);
                //dir = dir.normalized;
                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, turretRange, -5, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log(hit.transform.gameObject);
                    var parentTrans = hit.transform;
                    while (parentTrans.parent != null) { parentTrans = parentTrans.parent; }
                    var parentObj = parentTrans.gameObject;

                    var health = parentObj.GetComponentInChildren<AMS_Health_Management>();
                    if (health == target)
                    {
                        //Debug.Log("health == target");
                        if (leading || cheatLeading)
                        {
                            //find the highest parent transform
                            var aimPoint = target.transform;
                            while (aimPoint.parent != null) { aimPoint = aimPoint.parent; }

                            var charControl = aimPoint.gameObject.GetComponentInChildren<CharacterController>();
                            if (charControl != null)
                            {
                                var pos = AI_StaticFunctions.LeadTargetPosition(transform.position, Vector3.zero, bulletSpeed, charControl.transform.position, charControl.velocity);
                                dir = pos - transform.position;
                            }
                            else
                            {
                                var agent = aimPoint.gameObject.GetComponentInChildren<NavMeshAgent>();
                                if (agent != null)
                                {
                                    var pos = AI_StaticFunctions.LeadTargetPosition(transform.position, Vector3.zero, bulletSpeed, agent.transform.position, agent.velocity);
                                    dir = pos - transform.position;
                                }
                            }
                            leading = false;
                        }

                        var rotationSpeed = (turretCurrentRotationSpeed + turretRotationBuff);
                        turretRotationBuff = 0f;
                        rotationSpeed *= Time.deltaTime;
                        var targetRotation = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

                        if (Vector3.Angle(transform.forward, dir) < deadZone)
                        {
                            shotTimerCurrent -= Time.deltaTime;
                            if (shotTimerCurrent <= 0)
                            {
                                shotTimerCurrent = shotCurrentTimerMax;
                                shotCurrentTimerMax = shotBaseTimerMax;
                                ShootBullet(dir);
                            }
                        }
                    }
                    //else { Debug.Log("whatever was hit didn't have a health"); }

                }
            }
        }

        if (partTimer > 0f)
        {
            partTimer -= Time.deltaTime;
            SwitchParticles(true);
        }
        else
        {
            partTimer = 0f;
            SwitchParticles(false);
        }
    }

    private void ShootBullet(Vector3 direction)
    {
        if (ammoCurrent > 0)
        {
            GameObject Tempbullet = Instantiate(currentBullet, transform.position, Quaternion.identity);
            Tempbullet.transform.rotation = transform.rotation;

            var bulletScript = Tempbullet.GetComponent<AMS_Bullet>();
            if (bulletScript != null)
            {
                bulletScript.lockToY = false;
                bulletScript.speed = bulletSpeed;
                bulletScript.spread = bulletSpread;
                bulletScript.maxDistance = bulletTimer;
                bulletScript.contactDamage = bulletDamage;
                bulletScript.dontDamagePlayer = dontHurtPlayer;
                FireSoundEffect();
                bulletScript.type = "Turret";
            }
            else { Debug.Log("can't find bullet script to modify"); }

            var trail = Tempbullet.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                Gradient gradient;
                GradientColorKey[] colorKey;
                GradientAlphaKey[] alphaKey;

                gradient = new Gradient();

                // Populate the color keys at the relative time 0 and 1 (0 and 100%)
                colorKey = new GradientColorKey[2];
                colorKey[0].color = bulletColor;
                colorKey[0].time = 0.0f;
                colorKey[1].color = bulletColor;
                colorKey[1].time = 1.0f;

                // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
                alphaKey = new GradientAlphaKey[2];
                alphaKey[0].alpha = 1.0f;
                alphaKey[0].time = 0.0f;
                alphaKey[1].alpha = 1.0f;
                alphaKey[1].time = 1.0f;

                gradient.SetKeys(colorKey, alphaKey);

                trail.colorGradient = gradient;
            }
            else { Debug.Log("bullet does not have a trail renderer"); }

            if (usesAmmo) { ammoCurrent--; }
        }
    }//bang

    private void FireSoundEffect()
    {
        if (turretAudio != null)
        {
            if (!turretAudio.isPlaying)
            {
                turretAudio.Play();
            }
        }
    }

    protected virtual AMS_Health_Management AquireTarget()
    {
        AMS_Health_Management[] enemyArray = FindObjectsOfType<AMS_Health_Management>();
        if (enemyArray.Length > 0)
        {
            float minDistance = turretRange;
            AMS_Health_Management closestCont = null;
            foreach (AMS_Health_Management enemy in enemyArray)
            {
                if (enemy.tag == "Default_Enemy" && enemy.currentHealth > 0)
                {
                    var dis = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dis < minDistance)
                    {
                        minDistance = dis;
                        closestCont = enemy;
                    }
                }
            }
            if (closestCont != null)
            {
                return (closestCont);
            }
            else { return (null); }// Debug.Log("no enemy in range"); }
        }//there are enemies look to see if you hit
        else { return (null); }// Debug.Log("No enemies found in scene"); }
    }

    public void SwitchParticles(bool turnOn)
    {
        if(particleObject != null)
        {
            if(particleObject.activeSelf != turnOn)
            {
                particleObject.SetActive(turnOn);
            }
            //else { Debug.Log("Already set"); }
        }
        else
        {
            var partSys = GetComponentInChildren<ParticleSystem>();
            if(partSys != null) { particleObject = partSys.gameObject; }
        }
    }
}
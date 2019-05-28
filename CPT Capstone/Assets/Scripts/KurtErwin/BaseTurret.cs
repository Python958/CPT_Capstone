using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : FriendlyTurret
{
    private int lastUpgradeLevel = 1;

    //Holds reference to the player for upgrades
    private KE_MainPlayer_Script mainPlayerScript;

    protected override void Start()
    {
        turretRange = (turretRange > 0f) ? turretRange : 40f;
        turretBaseRotationSpeed = (turretBaseRotationSpeed > 0f) ? turretBaseRotationSpeed : 120f;
        turretCurrentRotationSpeed = turretBaseRotationSpeed;
        shotBaseTimerMax = (shotBaseTimerMax > 0f) ? shotBaseTimerMax : .4f;
        shotCurrentTimerMax = shotBaseTimerMax;

        bulletSpeed = (bulletSpeed > 0f) ? bulletSpeed : 18f;
        bulletTimer = (bulletTimer > 0f) ? bulletTimer : 3f;
        bulletDamage = (bulletDamage > 0) ? bulletDamage : 15;


        base.Start();
        mainPlayerScript = FindObjectOfType<KE_MainPlayer_Script>(); //assigns the variable so that upgrades can happen
        lastUpgradeLevel = mainPlayerScript.passiveUpgradeLevels[1];
    }

    protected override void Update()
    {
        if(mainPlayerScript.passiveUpgradeLevels[1] != lastUpgradeLevel) { TurretUpgrades(); }

        base.Update();
    }


    private void TurretUpgrades()
    {
        Debug.Log("applied turret upgrades");
        var upgradeLevel = mainPlayerScript.passiveUpgradeLevels[1];
        lastUpgradeLevel = upgradeLevel;
        if (upgradeLevel == 1)
        {
            turretBaseRotationSpeed = 20;
            shotBaseTimerMax = 1;
            bulletSpeed = 20;
        }
        if (upgradeLevel == 2)
        {
            turretBaseRotationSpeed = 40;
            shotBaseTimerMax = 0.8f;
            bulletSpeed = 30;
        }
        if (upgradeLevel == 3)
        {
            turretBaseRotationSpeed = 70;
            shotBaseTimerMax = 0.6f;
            bulletSpeed = 35;
        }
        if (upgradeLevel == 4)
        {
            turretBaseRotationSpeed = 100;
            shotBaseTimerMax = 0.4f;
            bulletSpeed = 40;
        }
    }
}

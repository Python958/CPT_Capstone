using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ConsoleDebugScript : MonoBehaviour
{
    public InputField userInput;
    public GameObject skyBombs;
    public AMS_BuyControls buyMenu;

    private bool activated = false;
    private KeyCode debugKey = KeyCode.BackQuote;

    // Start is called before the first frame update
    void Start()
    {
        exitDebugging();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            if (!activated)
            {
                togglePlayer(false);
             
                activated = true;
                userInput.gameObject.SetActive(true);
                userInput.ActivateInputField();
            }
            else { exitDebugging(); }
        }
        else if (!userInput.isFocused && activated == true) { exitDebugging(); }
    }

    private void exitDebugging()
    {
        togglePlayer(true);

        activated = false;
        debugCheck(userInput.text);
        userInput.text = "";
        userInput.gameObject.SetActive(false);
        userInput.DeactivateInputField();
    }

    private void togglePlayer(bool turnOn)
    {
        var playerMovement = GameObject.FindObjectOfType<KE_MainPlayer_Script>();
        if(playerMovement != null) { playerMovement.enabled = turnOn; }
        else { Debug.Log("can't find KE_MainPLayer_Script"); }

        var GunManagement = GameObject.FindObjectOfType<AMS_GunManagement>(); // Added to disable Reload Functionality when Player typed "R"
        if (GunManagement != null) { GunManagement.enabled = turnOn; }

        var OffHandWeapon = GameObject.FindObjectOfType<AMS_OffHand_Weapon>(); // Added to disable Off Hand Functionality when Player typed "1"
        if (OffHandWeapon != null) { OffHandWeapon.enabled = turnOn; }
    }

    private void debugCheck(string debugEntry)
    {
        debugEntry = debugEntry.ToLower();

        var numString = Regex.Match(debugEntry, @"\d+").Value;
        int.TryParse(numString, out int numInString);
        //Debug.Log(numInString);


        if (debugEntry.Contains("god")) { MakePlayerInvulnerable(); }
        else if (debugEntry.Contains("hurt"))
        {
            if (numInString != 0) { DamagePlayer(numInString); }
            else { DamagePlayer(30); }
        }
        else if (debugEntry.Contains("romeo")) { KillPlayer(); }
        else if (debugEntry.Contains("groundhog")) { RestartLevel(); }
        else if (debugEntry.Contains("ashes")) { KillAllEnemy(); }
        else if (debugEntry.Contains("step")) { NextCheckpoint(); }
        else if (debugEntry.Contains("trumped"))
        {
            if (numInString != 0) { AddResources(numInString); }
            else { AddResources(2016); }
        }
        else if (debugEntry.Contains("creative")) { ToggleSpawners(); }
        else if (debugEntry.Contains("reset")) { ZeroizeTiers(); }
        else if (debugEntry.Contains("jackpot")) { MaxTiers(); }
        else if (debugEntry.Contains("loadout"))
        {
            if (numInString != 0) { AmmoChange(numInString); }
            else { AmmoChange(1); }
        }
        else if (debugEntry.Contains("locksmith")) { OpenDoors(); }
        else if (debugEntry.Contains("hailmary")) { TurretBoost(); }
        else if (debugEntry.Contains("zerowing")) { BaseInvulnerable(); }
        else if (debugEntry.Contains("impatient")) { BeatLevel(); }
        else if (debugEntry.Contains("chickenlittle")) { SkyIsFalling(); }
        else if (debugEntry.Contains("triplewhammy"))
        {
            if (numInString != 0) { PowerTripleShot(numInString); }
            else { PowerTripleShot(10f); }
        }
        else if (debugEntry.Contains("hastalavista"))
        {
            if (numInString != 0) { PowerRapidFire(numInString); }
            else { PowerRapidFire(10f); }
        }
        else if (debugEntry.Contains("konami"))
        {
            if (numInString != 0) { PowerHollowPoint(numInString); }
            else { PowerHollowPoint(10f); }
        }
        else if (debugEntry.Contains("hollywood"))
        {
            if (numInString != 0) { PowerInfiniteAmmo(numInString); }
            else { PowerInfiniteAmmo(10f); }
        }
        else if (debugEntry.Contains("need it now")) { ResetAbilityCooldowns(); }
        else if (debugEntry.Contains("entitled")) { GiveAllAbilities(); }
        else if (debugEntry.Contains("goldeneye")) { MaxPlayerDamage(); }
        else if (debugEntry.Contains("construction"))
        {
            if(numInString != 0) { FillBaseHealth(numInString); }
            else { FillBaseHealth(500); }
        }
        else if (debugEntry.Contains("replicant"))
        {
            if (numInString != 0) { SetPlayerPassiveStats(numInString); }
            else { SetPlayerPassiveStats(4); }
        }
        else if (debugEntry.Contains("update")) { ToggleTurretSnipers(); }
        else if (debugEntry.Contains("offline")) { ToggleTurrets(); }
        else if (debugEntry.Contains("ninja")) { ZeroWaveTime(); }
        else if (debugEntry.Contains("vegas"))
        {
            if(numInString != 0) { AddScore(numInString); }
            else { AddScore(100000000); }
        }
    }

    private void MakePlayerInvulnerable()
    {
        var player = GameObject.Find("MainPlayer");
        if(player != null)
        {
            var health = player.GetComponent<AMS_Health_Management>();
            if(health != null)
            {
                health.ToggleInvulnerable();
                Debug.Log("invulnerability toggled");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void DamagePlayer(int damage)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var health = player.GetComponent<AMS_Health_Management>();
            if (health != null)
            {
                health.TakeDamageUnfair(damage);
                Debug.Log("Tis but a scratch");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void KillPlayer()
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var health = player.GetComponent<AMS_Health_Management>();
            if (health != null)
            {
                health.TakeDamageUnfair((int)health.currentHealth+1);
                Debug.Log("Thy drugs are quick");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void RestartLevel()
    {
        Debug.Log("I got you babe");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void KillAllEnemy()
    {
        GameObject[] enemy1 = GameObject.FindGameObjectsWithTag("Default_Enemy");
        for(var i = 0; i < enemy1.Length; i++)
        {
            if(enemy1[i] != null) { Destroy(enemy1[i]); }
        }

        GameObject[] enemy2 = GameObject.FindGameObjectsWithTag("Enemy");
        for (var i = 0; i < enemy2.Length; i++)
        {
            if (enemy2[i] != null) { Destroy(enemy2[i]); }
        }
        Debug.Log("Dust");
    }

    private void NextCheckpoint()
    {
        var checkPointManager = GameObject.FindObjectOfType<DLC_CheckPointController>();
        if(checkPointManager != null)
        {
            var nextCheck = checkPointManager.FindNextCheckPoint();
            var player = FindObjectOfType<KE_MainPlayer_Script>();
            if(player != null)
            {
                checkPointManager.UpdateCheckpoints(nextCheck);

                player.WarpTo(nextCheck.transform.position);
                Debug.Log("One giant leap");
            }
            else { Debug.Log("can't find the player"); }
        }
        else { Debug.Log("can't find checkpoint manager"); }

    }

    private void MaxResources()
    {
        var resourceController = GameObject.FindObjectOfType<AMS_ResourceController>();
        if(resourceController != null)
        {
            resourceController.currentResources = 2000;
            Debug.Log("Tweet that wall down");
        }
        else { Debug.Log("can't find resource controller"); }
    }

    private void AddResources(int value)
    {
        var resourceController = GameObject.FindObjectOfType<AMS_ResourceController>();
        if (resourceController != null)
        {
            resourceController.currentResources += value;
            Debug.Log("Tweet that wall down");
        }
        else { Debug.Log("can't find resource controller"); }
    }

    private void ToggleSpawners()
    {
        TriggerEnemyScript[] triggers = GameObject.FindObjectsOfType<TriggerEnemyScript>();

        if(triggers.Length > 0)
        {
            Debug.Log("build in peace");
            foreach (TriggerEnemyScript trigger in triggers)
            {
                trigger.allowedToTrigger = !trigger.allowedToTrigger;
            }
        }
        else { Debug.Log("can't find any enemy triggers"); }
    }

    private void ZeroizeTiers()
    {
        var playerBuy = FindObjectOfType<AMS_BuyMenuPlayer>();
        if(playerBuy != null)
        {
            var buyMenuObj = playerBuy.buyMenuGUI;
            if(buyMenuObj != null)
            {
                var buyMenu = buyMenuObj.GetComponent<AMS_BuyControls>();
                if(buyMenu != null)
                {
                    buyMenu.levelInts[0] = 1;
                    buyMenu.levelInts[1] = 1;
                    buyMenu.levelInts[2] = 1;
                    buyMenu.levelInts[3] = 1;
                    Debug.Log("Vizzini said go back to the beginning");
                }
                else { Debug.Log("can't find component on buy obj"); }
            }
            else { Debug.Log("can't find buy object"); }
        }
        else { Debug.Log("can't find player buy component"); }
    }

    private void MaxTiers()
    {
        var playerBuy = FindObjectOfType<AMS_BuyMenuPlayer>();
        if (playerBuy != null)
        {
            var buyMenuObj = playerBuy.buyMenuGUI;
            if (buyMenuObj != null)
            {
                var buyMenu = buyMenuObj.GetComponent<AMS_BuyControls>();
                if (buyMenu != null)
                {
                    buyMenu.levelInts[0] = 3;
                    buyMenu.levelInts[1] = 3;
                    buyMenu.levelInts[2] = 3;
                    buyMenu.levelInts[3] = 3;
                    Debug.Log("in like Flynn");
                }
                else { Debug.Log("can't find component on buy obj"); }
            }
            else { Debug.Log("can't find buy object"); }
        }
        else { Debug.Log("can't find player buy component"); }
    }

    private void AmmoChange(int numOfMags)
    {
        var player = FindObjectOfType<KE_MainPlayer_Script>().gameObject;
        if(player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if(gun!= null)
            {
                var i = 0;
                while (i < numOfMags)
                {
                    if (gun.currentGun == "Default") { gun.AddAmmo("Default"); }
                    else { gun.AddAmmo("Unlock"); }

                    i++;
                }
                
                Debug.Log("Feinstein just fainted");
            }
            else { Debug.Log("can't find gun on player"); }
        }
        else { Debug.Log("Can't find player"); }
    }

    private void OpenDoors()
    {
        var levelGates = FindObjectOfType<LevelGateScript>();
        if (levelGates != null)
        {
            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
            for (int i = 0; i < allDoors.Length; i++)
            {
                allDoors[i].GetComponent<LevelGateScript>().OpenDoor();
                Debug.Log("Open Says Me");
            }
        }
        else
        {
            Debug.Log("Can't find Level Gate in the scene");
        }
    }

    private void TurretBoost()
    {
        var turrets = FindObjectsOfType<BaseTurret>();
        if(turrets.Length > 0)
        {
            Debug.Log("now and at the moment of our death");
            foreach (BaseTurret turret in turrets)
            {
                turret.turretBaseRotationSpeed = 180;
                turret.shotBaseTimerMax = .1f;
                turret.bulletSpeed = 70f;
            }
        }
    }

    private void BaseInvulnerable()
    {
        BaseHealth[] bases;
        bases = FindObjectsOfType<BaseHealth>();
        if(bases.Length > 0)
        {
            foreach(BaseHealth theBase in bases)
            {
                theBase.invulnerable = !theBase.invulnerable;
            }
            Debug.Log("All your base are belong to us");
        }
        else { Debug.Log("no bases on level"); }
    }

    private void BeatLevel()
    {
        Debug.Log("I guess cheetahs do win");
        AMS_UniversalFunctions.GoToResultsScreen(true);
    }

    private void SkyIsFalling()
    {
        var bomber = FindObjectOfType<RandomBomberScript>();
        if(bomber != null) { Destroy(bomber.gameObject); Debug.Log("They all go in, but they never, never come out again"); }
        else if (skyBombs != null)
        {
            Debug.Log("The sky is falling!");
            var pos = new Vector3(0f, 0f, 0f);
            Instantiate(skyBombs, pos, Quaternion.identity);
        }
    }

    private void PowerTripleShot(float value)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if (gun != null)
            {
                gun.powerup = "Trishot";
                gun.powerupLength = value;
                gun.powerupMax = value;
                Debug.Log("I AM the LAW");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void PowerRapidFire(float value)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if (gun != null)
            {
                gun.powerup = "RapidFire";
                gun.powerupLength = value;
                gun.powerupMax = value;
                Debug.Log("Look, I'm the governa!");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void PowerHollowPoint(float value)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if (gun != null)
            {
                gun.powerup = "HollowPoint";
                gun.powerupLength = value;
                gun.powerupMax = value;
                Debug.Log("Up, Up, Down, Down, Left, Right, Left, Right, B, A");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void PowerInfiniteAmmo(float value)
    {
        var player = GameObject.Find("MainPlayer");
        if (player != null)
        {
            var gun = player.GetComponent<AMS_GunManagement>();
            if (gun != null)
            {
                gun.powerup = "InfiniteAmmo";
                gun.powerupLength = value;
                gun.powerupMax = value;
                Debug.Log("Reloading optional");
            }
            else { Debug.Log("can't find health component on player"); }
        }
        else { Debug.Log("can't find player"); }
    }

    private void ResetAbilityCooldowns()
    {
        var controller = FindObjectOfType<AMS_BuyMenuCooldownController>();
        if(controller != null)
        {
            Debug.Log("all systems nominal");

            controller.currentammoBoxCooldown = 0;
            controller.currentBaseRestoreCooldown = 0;
            controller.currentDropBombCooldown = 0;
            controller.currenthealthPackCooldown = 0;
            controller.currentminesCooldown = 0;
            controller.currentSpikeTrapCooldown = 0;
            controller.currentUnlockAmmoCooldown = 0;
            controller.currentWeaknessZoneCooldown = 0;
        }
        else { Debug.Log("can't find cool down controller"); }
    }

    private void GiveAllAbilities()
    {
        if(buyMenu != null)
        {
            Debug.Log("Only in America");

            for(var i = 0; i < buyMenu.unlockedAbilities.Length; i++)
                { buyMenu.UnlockAbility(i, 0); }
        }
        else { Debug.Log("can't find buy menu"); }
    }

    private void MaxPlayerDamage()
    {
        var gun = FindObjectOfType<AMS_GunManagement>();
        if(gun != null)
        {
            Debug.Log("Sorry about the leg. Skiing? ...Hunting!!!");
            gun.goldenBullet = !gun.goldenBullet;
        }
        else { Debug.Log("can't find player gun"); }
    }

    private void FillBaseHealth(int amount)
    {
        var homeBase = FindObjectOfType<BaseHealth>();
        if(homeBase != null)
        {
            Debug.Log("Hammers and nails");
            var returnHP = Mathf.Max(0f, homeBase.currentHP + amount);
            returnHP = Mathf.Min(homeBase.maxHP, returnHP);
            homeBase.currentHP = returnHP;
        }
        else { Debug.Log("can't find the base health component"); }
    }

    private void SetPlayerPassiveStats(int value)
    {
        var player = FindObjectOfType<KE_MainPlayer_Script>();
        if(player != null && buyMenu != null)
        {
            var size = buyMenu.passiveLevelInts.Length;
            for (var i = 0; i < size; i++)
            {
                buyMenu.passiveLevelInts[i] = Mathf.Clamp(value, 0, 4);
                if(value == 4) { buyMenu.passiveUpgradeButton[i].SetActive(false); }
                else { buyMenu.passiveUpgradeButton[i].SetActive(true); }
            }
            player.passiveUpgradeLevels = buyMenu.passiveLevelInts;
            Debug.Log("I want more life");
        }
        else { Debug.Log("can't find player or buy menu"); }
    }

    private void ToggleTurretSnipers()
    {
        var turrets = FindObjectsOfType<FriendlyTurret>();
        if(turrets.Length > 0)
        {
            Debug.Log("downloading turretOS patch 1.03.2");

            bool flag = !turrets[0].cheatLeading;
            foreach (FriendlyTurret turret in turrets)
            {
                turret.cheatLeading = flag;
            }
        }
        else { Debug.Log("no turrets found"); }
    }

    private void ToggleTurrets()
    {
        var turrets = FindObjectsOfType<ParentTurret>();
        if(turrets.Length > 0)
        {
            var flag = !turrets[0].offline;

            foreach (ParentTurret turret in turrets)
            {
                turret.offline = flag;
            }
            Debug.Log("Skynet offline");
        }
        else { Debug.Log("no turrets found"); }
    }

    public void ZeroWaveTime()
    {
        var waveScript = FindObjectOfType<WaveSpawner>();
        if(waveScript != null)
        {
            Debug.Log("You didn't see anything!!");
            if (waveScript.currentWaveTime > 0) { waveScript.currentWaveTime = .1f; }
            else
            {
                waveScript.currentWaveTime = -(waveScript.coolDownTimer - .1f);
            }
        }
    }

    private void AddScore(int value)
    {
        Debug.Log("this time the house LOSES!");
        AMS_ScoreController.increaseScore(value);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //DLC - Added UI Functionality

public class AMS_GunManagement : MonoBehaviour
{
    public GameObject currentBullet;
    public GameObject spawnLocation;


    //Default Gun Alternative Projectile
    public bool alternativeFireEnabled = true;
    public GameObject defaultAlternateGrenade;
    public float defaultAlternativeMaxFireRate = 1;
    public float defaultAlternativeCurrentFireRate;
    public int defaultAlternateFireAmmo;
    public int maxDefaultAlrernateFireAmmo = 5;
    public string currentGun;


    //Starting Weapon
    public string gun1;
    public int gun1maxAmmo;
    public int gun1currentAmmo;
    public int gun1currentClips;
    public float gun1maxFireRate;
    public GameObject gun1Bullet;
    //Unlocked Weapon
    [HideInInspector]
    public bool unlocked = false;
    [HideInInspector]
    public string gun2 = null;
    [HideInInspector]
    public int gun2maxAmmo;
    [HideInInspector]
    public int gun2currentAmmo;
    [HideInInspector]
    public int gun2currentClips;
    [HideInInspector]
    public float gun2maxFireRate;
    [HideInInspector]
    public GameObject gun2Bullet = null;



    //Shared gun variables
    public int currentAmmo;
    public int maxAmmo;
    public int currentClips;                    //DLC Made it Public to see the current clip
    public int maxClips;
    public float maxReloadTime;
    private float currentReloadTime;
    private float maxFireRate;
    private float currentFireRate;
    public GameObject laser;

    //Gun Sound Effects
    public AudioClip defaultSound;
    public AudioClip shotgunSound;
    public AudioClip railGunSound;
    public AudioClip ricochetRifleSound;
    public AudioClip outOfAmmo;
    public AudioClip reloadSound;

    //Reload info
    private bool currentlyFiring = false;
    public float fastReloadTimeChange;          //this is a PERCENTAGE of time that is either added or subtracted from the reload if you succeed or fail at fast reloading
    public float fastReloadMarker;              //this is a PERCENTAGE that marks the beginning of where the fast reload is suppose to be pressed
    public float fastReloadSpread;              //this is a PERCENTAGE that shows how wide the space is that the fast reload can be performed
    //NOTE the fast reload marker added to spread should not be greater than the TimeChange or it's pointless.

    private ReloadGUIScript reloadGUI;

    private bool addAmmo;

    private KeyCode reloadKey = KeyCode.R;
    [HideInInspector] public bool reloading = false;
    [HideInInspector] public bool fastReloadAttempted = false;
    [HideInInspector] public bool fastReloadSuccess = false;
    public bool gunEnabled = true;

    public Text ammoText;                       //DLC Added public text variable for primary  
    public Text AlternateAmmoText;              //Grenade Launcher Text
    public GameObject[] ammoClip;               // DLC - AMMO Clip array
    public GameObject PrimaryWeaponGUI;
    public GameObject SecondaryWeaponGUI;
    public Color ActiveWeaponColor;
    public Color InactiveWeaponColor;
    public Color WeaponTextNameColor;
    public Text PrimaryActiveWeaponName;
    public Text SecondaryActiveWeaponName;
    public GameObject ActiveWeapon;
    public GameObject AlternateAmmoImage;
    public GameObject[] bulletTypes;

    //Powerup
    [HideInInspector]
    public string powerup = "Inactive";
    [HideInInspector]
    public float powerupLength = 0;
    private Color powerupLerpColor = Color.white;
    public MeshRenderer playerBody;
    public Image powerupDisplay; //Image for the power up timer.
    private float powerupDisplayAmount;
    [HideInInspector]
    public float powerupMax;
    [HideInInspector]
    public bool goldenBullet = false;
    public GameObject OutOfAmmo; //OutofAmmo Notification
    private KE_MainPlayer_Script mainPlayerScript;

    //Animations
    public Animation reload;
    public Animation switchGun;

    //Gun Models
    public GameObject defaultGun;
    public GameObject railGun;
    public GameObject ricochetRifle;
    public GameObject shotGun;
    public bool modelSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        //General Gun Setup
        currentGun = gun1;
        maxAmmo = gun1maxAmmo;
        currentAmmo = maxAmmo;
        currentClips = gun1currentClips;
        maxFireRate = gun1maxFireRate;
        currentBullet = gun1Bullet;
        currentFireRate = maxFireRate;

        //Gun 1 Setup
        gun1currentAmmo = maxAmmo;
        //Laser Setup
        LaserColor();


        defaultAlternateFireAmmo = maxDefaultAlrernateFireAmmo;
        defaultAlternativeCurrentFireRate = defaultAlternativeMaxFireRate * 2;
        AlternateAmmoText = GameObject.Find("Alternate_Ammo_Text").GetComponent<Text>(); //Grabbing the text so it doesn't need to be assigned in the inspector
        PrimaryWeaponGUI = GameObject.Find("PrimaryWeapon"); //Primary Weapon GUI
        SecondaryWeaponGUI = GameObject.Find("SecondaryWeapon"); // Secondary Weapon GUI
        SecondaryWeaponGUI.GetComponentInChildren<Text>().color = InactiveWeaponColor;
        SecondaryWeaponGUI.SetActive(false);
        AlternateAmmoImage = GameObject.Find("Grenade"); // Finds the Grenade Image 
        PrimaryActiveWeaponName = GameObject.Find("PrimaryActiveWeaponName").GetComponent<Text>();
        SecondaryActiveWeaponName = GameObject.Find("SecondaryActiveWeaponName").GetComponent<Text>();
        ActiveWeapon = GameObject.Find("ActiveWeapon"); // Find the Active Weapon Game Object on the Main Camera Prefab / PlayerGUI/ Canvas
        ActiveWeapon.SetActive(false);
        PrimaryActiveWeaponName.enabled = true;
        SecondaryActiveWeaponName.enabled = true;
        SecondaryActiveWeaponName.GetComponentInChildren<Text>().color = InactiveWeaponColor;
        powerupDisplay = GameObject.Find("Powerup_GUI").GetComponent<Image>();
        powerupDisplay.gameObject.SetActive(false); // turns off the powerup display on start. 
        OutOfAmmo = GameObject.Find("OutOfAmmo");
        OutOfAmmo.SetActive(false);
        mainPlayerScript = GetComponent<KE_MainPlayer_Script>();

        if (defaultGun != null)
        {
            defaultGun.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        ActiveWeaponText();
        ActivePowerup();
        if (gunEnabled)
        {
            OutOfAmmo.SetActive(false);
            currentlyFiring = false;
            UpdateClipsGUI();
            //Allows player to switch weapons
            WeaponSwitch();
            if (Input.GetMouseButton(0)  && !reloading)
            {
                if (currentGun == "Default" || currentGun == "Rail Gun" || currentGun == "Ricochet Rifle" || currentGun == "Shotgun")
                {
                    if (currentAmmo > 0 || powerup == "InfiniteAmmo")
                    {
                        if (currentFireRate > maxFireRate || powerup == "RapidFire" && currentFireRate * 2 > maxFireRate)
                        {
                            currentFireRate = 0;
                            WeaponSound();

                            //deals with trishot ability to shoot multiples
                            int shots = (powerup == "Trishot") ? 3 : 1;

                            if (currentGun == "Shotgun")
                            { shots = (powerup == "Trishot") ? 20 : 10; }

                            for (int i = 0; i < shots; i++)
                            { CreateBullet(); }//create how ever many bullets you should

                            if (powerup != "InfiniteAmmo") { currentAmmo--; }
                        }
                        currentlyFiring = true;
                    }
                    else
                    //No Ammo
                    {
                        EmptySound();
                        if (currentClips > 0 && !reloading && defaultAlternativeCurrentFireRate >= defaultAlternativeMaxFireRate)
                        {
                            StartReload();
                        }
                    }
                }
            }
            //Middle Mouse Press
            else if (Input.GetMouseButtonDown(2) && !reloading || Input.GetKeyDown(KeyCode.Space) && !reloading) //|| Input.GetKeyDown(KeyCode.LeftControl) && !reloading || Input.GetKeyDown(KeyCode.RightControl) && !reloading)
            {
                if (currentGun == "Default")
                {
                    if (alternativeFireEnabled)
                    {
                        AlternativeFireFragGrenade();
                    }

                }
            }
            //shooting

            else if (Input.GetKeyDown(reloadKey)
                && currentClips > 0
                && currentAmmo < maxAmmo
                && !reloading
                && defaultAlternativeCurrentFireRate >= defaultAlternativeMaxFireRate)
            {
                StartReload();
            }//sets up the beginning of the reload

            else if (reloading)
            {
                if (currentReloadTime < maxReloadTime)
                {
                    if (!fastReloadAttempted && Input.GetKeyDown(reloadKey))
                    {
                        if (reload != null) { reload.Play(); }
                        fastReloadAttempted = true;
                        var goalTimeBegin = maxReloadTime * fastReloadMarker;
                        var goalTimeEnd = goalTimeBegin + (maxReloadTime * fastReloadSpread);
                        var timeChangedForReload = maxReloadTime * fastReloadTimeChange;

                        if (currentReloadTime > goalTimeBegin && currentReloadTime < goalTimeEnd)
                        {
                            currentReloadTime += timeChangedForReload;
                            fastReloadSuccess = true;
                        }//fast reload succeeded

                        else
                        {
                            currentReloadTime -= timeChangedForReload;
                            fastReloadSuccess = false;
                        }//fast reload failed

                    }//this deals with the fast reload
                    ReloadSpeedCheck();
                }//reloading count down and fast reload

                else
                {
                    reloading = false;
                    FillGun();
                }//reloading end reached

            }//reloading process

        }//all the gun interaction

        if (currentFireRate <= maxFireRate) currentFireRate += 1 * Time.deltaTime; //"if" so it doesn't count up indefinitely
        if (defaultAlternativeCurrentFireRate <= defaultAlternativeMaxFireRate) defaultAlternativeCurrentFireRate += 1 * Time.deltaTime;
        ammoText.text = "" + currentAmmo.ToString("000"); // DLC - GUI Text String Displayed for ammo
        AlternateAmmoText.text = "" + defaultAlternateFireAmmo.ToString("00"); // DLC - GUI Text String Displayed for Alternate Fire ammo

        if (modelSwitch == true)
        {
            if(ricochetRifle !=  null)
            {
                ricochetRifle.SetActive(currentGun == "Ricochet Rifle");
            }
            if (shotGun != null)
            {
                shotGun.SetActive(currentGun == "Shotgun");
            }
            if (railGun != null)
            {
                railGun.SetActive(currentGun == "Rail Gun");
            }
            if(defaultGun != null)
            {

                defaultGun.SetActive(currentGun == "Default");
            }
            modelSwitch = false;
        }
        //Debug.Log("current gun is Default");
    }

    public bool AddAmmo(string type)
    {
        addAmmo = false;
        if (type == "Default")
        {
            if (currentGun == "Default")
            {
                if (currentClips < maxClips)
                {
                    ammoClip[currentClips++].SetActive(true); //DLC This adds GUI functionality to the getting an ammo clip from an ammo box.
                                                              //currentClips++;  DLC - Commented this out for the line noted above.
                    addAmmo = true;
                }
            }
            else if (gun1currentClips < maxClips)
            {
                gun1currentClips++;
                addAmmo = true;
            }//reload the gun even though not active
        }
        if (type == "Unlock")
        {
            if (currentGun != "Default")
            {
                if (currentClips < maxClips)
                {
                    ammoClip[currentClips++].SetActive(true); //DLC This adds GUI functionality to the getting an ammo clip from an ammo box.
                                                              //currentClips++;  DLC - Commented this out for the line noted above.
                    addAmmo = true;
                }
            }
            else if (gun2currentClips < maxClips)
            {
                gun2currentClips++;
                addAmmo = true;
            }//reload the gun even though not active
        }
        if (type == "Alternate")
        {
            if (defaultAlternateFireAmmo < maxDefaultAlrernateFireAmmo)
            {
                defaultAlternateFireAmmo++;

                addAmmo = true;
            }
        }

        return addAmmo;
    }

    public void ToggleGun(bool shouldBeEnabled)
    {
        gunEnabled = shouldBeEnabled;
        reloading = false;
    }

    private void FillGun()
    {
        if (currentClips > 0)
        {
            currentClips--;
            //DLC Added (Functionality to GUI Clips: Alien Swarm only had 5 Clips)
            if (ammoClip.Length < 0)
            {
                return;
            }
            else
            {
                //Debug.Log(currentClips);
                ammoClip[currentClips].SetActive(false); //DLC - Testing
            }
            MaxAmmoCheck();
            currentAmmo = maxAmmo;
            //Debug.Log("reloaded");
        }
    }

    //Below are the Alternative Fire Weapons
    private void AlternativeFireFragGrenade()
    {
        //Ammo Check
        if (defaultAlternateFireAmmo > 0)
        {
            //Off Cooldown
            if (defaultAlternativeCurrentFireRate >= defaultAlternativeMaxFireRate)
            {
                //Launch
                GameObject Tempbullet = Instantiate(defaultAlternateGrenade, new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y, spawnLocation.transform.position.z), Quaternion.identity);

                Tempbullet.transform.rotation = transform.rotation;
                defaultAlternateFireAmmo--;
                defaultAlternativeCurrentFireRate = 0;
            }
        }
    }

    public float ReloadPercComplete()
    {
        if (maxReloadTime != 0f)
        {
            var reloadPerc = currentReloadTime / maxReloadTime;
            return (reloadPerc);
        }
        else
        {
            Debug.Log("divide by zero attempt on reload percent");
            return (0f);
        }
    }//checks the private variables to return the percent of reload complete. Used by GUI

    public bool IsFiring()
    {
        return (currentlyFiring);
    }//public method to return private variable of for whether firing.

    public float AmmoPercent()
    {
        return ((float)currentAmmo / maxAmmo);
    }

    private void UpdateClipsGUI()
    {
        for (int i = 0; i < ammoClip.Length; i++)
        {
            if (i >= currentClips)
            {
                ammoClip[i].SetActive(false);
            }
            else
            {
                ammoClip[i].SetActive(true);
            }
        }
    }

    private void WeaponSwitch()
    {
        if (unlocked)
        {
            ActiveWeapon.SetActive(true);
            SecondaryWeaponGUI.SetActive(true);
            PrimaryWeaponGUI.SetActive(true);

            if (Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetKeyDown(KeyCode.F))
            {
                if(switchGun != null) { switchGun.Play(); }
                //Switch to gun 2
                if (currentGun == gun1)
                {
                    //save current variables for gun 1
                    gun1currentAmmo = currentAmmo;
                    gun1currentClips = currentClips;
                    //Change gun
                    currentGun = gun2;
                    maxAmmo = gun2maxAmmo;
                    currentAmmo = gun2currentAmmo;
                    currentClips = gun2currentClips;
                    maxFireRate = gun2maxFireRate;
                    currentFireRate = gun2maxFireRate;
                    currentBullet = gun2Bullet;

                    //Graphical Changes
                    LaserColor();
                    //SecondaryWeaponGUI.SetActive(true);
                    //PrimaryWeaponGUI.SetActive(false);
                    PrimaryWeaponGUI.GetComponentInChildren<Text>().color = InactiveWeaponColor;
                    SecondaryWeaponGUI.GetComponentInChildren<Text>().color = ActiveWeaponColor;
                    SecondaryActiveWeaponName.enabled = true;
                    PrimaryActiveWeaponName.enabled = true;
                    PrimaryActiveWeaponName.GetComponentInChildren<Text>().color = InactiveWeaponColor;
                    SecondaryActiveWeaponName.GetComponentInChildren<Text>().color = WeaponTextNameColor;
                    AlternateAmmoImage.SetActive(false); //Turns off alter weapon image when secondary weapon is out
                    AlternateAmmoText.gameObject.SetActive(false);  //Turns off alter weapon text when secondary weapon is out
                    modelSwitch = true;
                }


                //Switch to gun 1
                else
                {
                    //save current variables for gun 2
                    gun2currentAmmo = currentAmmo;
                    gun2currentClips = currentClips;

                    //Change gun
                    currentGun = gun1;
                    maxAmmo = gun1maxAmmo;
                    currentAmmo = gun1currentAmmo;
                    currentClips = gun1currentClips;
                    maxFireRate = gun1maxFireRate;
                    currentFireRate = gun2maxFireRate;
                    currentBullet = gun1Bullet;

                    //Graphical Changes
                    LaserColor();
                    //SecondaryWeaponGUI.SetActive(false);
                    //PrimaryWeaponGUI.SetActive(true);
                    PrimaryWeaponGUI.GetComponentInChildren<Text>().color = ActiveWeaponColor;
                    SecondaryWeaponGUI.GetComponentInChildren<Text>().color = InactiveWeaponColor;
                    SecondaryActiveWeaponName.enabled = true;
                    SecondaryActiveWeaponName.GetComponentInChildren<Text>().color = InactiveWeaponColor;
                    PrimaryActiveWeaponName.GetComponentInChildren<Text>().color = WeaponTextNameColor;
                    PrimaryActiveWeaponName.enabled = true;
                    AlternateAmmoImage.SetActive(true);
                    AlternateAmmoText.gameObject.SetActive(true);
                    modelSwitch = true;
                }
            }
        }
    }

    private void LaserColor()
    {
        if (currentGun == "Default")
        {
            laser.GetComponent<LineRenderer>().materials[0].color = Color.red;
        }
        if (currentGun == "Rail Gun")
        {
            laser.GetComponent<LineRenderer>().materials[0].color = Color.blue;
        }
        if (currentGun == "Shotgun")
        {
            laser.GetComponent<LineRenderer>().materials[0].color = Color.black;
        }
        if (currentGun == "Ricochet Rifle")
        {
            laser.GetComponent<LineRenderer>().materials[0].color = Color.green;
        }
    }

    private void WeaponSound()
    {
        var aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
        if (aSwitcher == null) { Debug.Log("can't find audio switcher"); }

        if (currentGun == "Default")
        {
            aSwitcher.PlaySound(defaultSound);
        }
        else if (currentGun == "Shotgun")
        {
            aSwitcher.PlaySound(shotgunSound);
        }
        else if (currentGun == "Rail Gun")
        {
            aSwitcher.PlaySound(railGunSound);
        }
        else if (currentGun == "Ricochet Rifle")
        {
            aSwitcher.PlaySound(ricochetRifleSound);
        }
        //No sound
        else
        {
            Debug.Log("audio not assigned");
        }

    }

    private void EmptySound()
    {
        var aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
        if (aSwitcher == null) { Debug.Log("can't find audio switcher"); }
        aSwitcher.PlaySound(outOfAmmo);
        OutOfAmmo.SetActive(true);
    }

    public void UnlockWeapon(string weapon)
    {
        unlocked = true;
        //Should not need to be Called but Default
        if (weapon == "Default")
        {
            gun2 = "Default";
            gun2maxAmmo = 120;
            gun2currentAmmo = gun2maxAmmo;
            gun2currentClips = 5;
            gun2maxFireRate = 0.123f;
            gun2Bullet = bulletTypes[0];
        }
        //Shotgun
        if (weapon == "Shotgun")
        {
            gun2 = "Shotgun";
            gun2maxAmmo = 12;
            gun2currentAmmo = gun2maxAmmo;
            gun2currentClips = 5;
            gun2maxFireRate = 0.8f;
            gun2Bullet = bulletTypes[1];
        }
        //Rail Gun
        if (weapon == "Rail Gun")
        {
            gun2 = "Rail Gun";
            gun2maxAmmo = 10;
            gun2currentAmmo = gun2maxAmmo;
            gun2currentClips = 5;
            gun2maxFireRate = .6f;
            gun2Bullet = bulletTypes[2];
        }
        //Ricochet Rifle
        if (weapon == "Ricochet Rifle")
        {
            gun2 = "Ricochet Rifle";
            gun2maxAmmo = 40;
            gun2currentAmmo = gun2maxAmmo;
            gun2currentClips = 5;
            gun2maxFireRate = .05f;
            gun2Bullet = bulletTypes[3];
        }
    }

    private void ActivePowerup()
    {
        if (powerup != "Inactive")
        {
            PowerUpHud();
            ColorChangingPowerup();
            //Reduces Duration
            powerupLength -= PowerupDuation() * Time.deltaTime;
            if (powerupLength <= 0)
            {
                powerup = "Inactive";
                powerupLerpColor = Color.white;
                playerBody.material.color = powerupLerpColor;
            }
        }
    }

    private void ColorChangingPowerup()
    {
        if (powerup == "RapidFire")
        {
            powerupLerpColor = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 2));
        }
        if (powerup == "HollowPoint")
        {
            powerupLerpColor = Color.Lerp(Color.blue, Color.green, Mathf.PingPong(Time.time, 2));
        }
        if (powerup == "Trishot")
        {
            powerupLerpColor = Color.Lerp(Color.black, Color.grey, Mathf.PingPong(Time.time, 2));
        }
        if (powerup == "InfiniteAmmo")
        {
            powerupLerpColor = Color.Lerp(Color.yellow, Color.cyan, Mathf.PingPong(Time.time, 2));
        }
        playerBody.material.color = powerupLerpColor;
    }

    private void PowerUpHud()
    {
        powerupDisplay.gameObject.SetActive(true);//turns on the powerup display. 
        powerupDisplay.fillAmount = powerupLength / powerupMax;
        //  Debug.Log("The current time remaining is " + powerupDisplayAmount);
       // Debug.Log("The current fill amount is " + powerupDisplay.fillAmount);
    }

    private void StartReload()
    {
        reloading = true;
        currentReloadTime = 0;
        fastReloadAttempted = false;
        var aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
        if (aSwitcher == null) { Debug.Log("can't find audio switcher"); }
        aSwitcher.PlaySound(reloadSound);
    }

    private void ReloadSpeedCheck()
    {
        if (mainPlayerScript.passiveUpgradeLevels[2] == 1)
        {
            currentReloadTime += 1 * Time.deltaTime;
        }
        if (mainPlayerScript.passiveUpgradeLevels[2] == 2)
        {
            currentReloadTime += 1.3f * Time.deltaTime;
        }
        if (mainPlayerScript.passiveUpgradeLevels[2] == 3)
        {
            currentReloadTime += 1.6f * Time.deltaTime;
        }
        if (mainPlayerScript.passiveUpgradeLevels[2] == 4)
        {
            currentReloadTime += 2f * Time.deltaTime;
        }
    }

    private void MaxAmmoCheck()
    {
        if (currentGun == "Default")
        {
            if (mainPlayerScript.passiveUpgradeLevels[3] == 1)
            {
                maxAmmo = 120;
            }
            if (mainPlayerScript.passiveUpgradeLevels[3] == 2)
            {
                maxAmmo = 160;
            }
            if (mainPlayerScript.passiveUpgradeLevels[3] == 3)
            {
                maxAmmo = 200;
            }
            if (mainPlayerScript.passiveUpgradeLevels[3] == 4)
            {
                maxAmmo = 240;
            }
        }

    }

    private float PowerupDuation()
    {
        if (mainPlayerScript.passiveUpgradeLevels[4] == 1)
        {
            return 1;
        }
        if (mainPlayerScript.passiveUpgradeLevels[4] == 2)
        {
            return .8f;
        }
        if (mainPlayerScript.passiveUpgradeLevels[4] == 3)
        {
            return .6f;
        }
        if (mainPlayerScript.passiveUpgradeLevels[4] == 4)
        {
            return .4f;
        }
        return 1;
    }

    private void ActiveWeaponText()
    {
        PrimaryActiveWeaponName.text = gun1;
        SecondaryActiveWeaponName.text = gun2;

    }

    public void CreateBullet()
    {
        GameObject Tempbullet = Instantiate(currentBullet, new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y, spawnLocation.transform.position.z), Quaternion.identity);
        Tempbullet.transform.eulerAngles = transform.eulerAngles;
        if (goldenBullet)
        {
            Tempbullet.GetComponentInChildren<AMS_Bullet>().contactDamage = 999;
        }
        else if (powerup == "HollowPoint")
        {
            Tempbullet.GetComponentInChildren<AMS_Bullet>().contactDamage = Tempbullet.GetComponentInChildren<AMS_Bullet>().contactDamage * 2;
        }
    }
}




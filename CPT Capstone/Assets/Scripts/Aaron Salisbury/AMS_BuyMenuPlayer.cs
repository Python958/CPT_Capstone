using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMS_BuyMenuPlayer: MonoBehaviour
{
    public GameObject buyMenuGUI;
    public KeyCode activation;
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine")]
    public KeyCode[] abilityKeys;
    public bool isOpen = false;
    public bool placing = false;    //a generic toggle anytime an ability is being placed
    private bool closing = false;   //a toggle so it knows when to pay attention to the mouse lift
    private AMS_BuyMenuCooldownController cooldownControl;

    //Abilities
    public GameObject ammoBox;
    [HideInInspector]
    public bool ammoBoxActive;
    public GameObject dropBomb;
    [HideInInspector]
    public bool bombActive;
    public GameObject healthPack;
    [HideInInspector]
    public bool healthPackActive;
    public GameObject mine;
    [HideInInspector]
    public bool minesActive;
    private int placeableMines;
    public string ammoBoxType;
    public AudioClip activateAbilitySound;
    public BaseHealth baseHealthScript;
    public GameObject weaknessZone;
    [HideInInspector]
    public bool weaknessZoneActive;
    [HideInInspector]
    public bool spikeTrapActive;
    public GameObject spikeTrap;
    [HideInInspector]
    public bool tauntTotemActive;
    public GameObject tauntTotem;


    // Start is called before the first frame update
    void Start()
    {
        cooldownControl = FindObjectOfType<AMS_BuyMenuCooldownController>().GetComponent<AMS_BuyMenuCooldownController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Use Ability without menu
        for (int i = 0; i < abilityKeys.Length; i++)
        {
            if (Input.GetKeyDown(abilityKeys[i]))
            {
                ActivateAbility(i);
            }
        }

        if (Input.GetKeyDown(activation))
        {
            if (!FindObjectOfType<MenuController>().isPaused)
            {
                if (isOpen)
                {
                    isOpen = false;
                    buyMenuGUI.GetComponent<AMS_BuyControls>().notEnoughMessage.SetActive(false);
                    buyMenuGUI.SetActive(false);
                    TogglePlayerFunctions(true);
                    buyMenuGUI.GetComponent<AMS_BuyControls>().CloseMenu(true);
                }
                else
                {
                    isOpen = true;
                    CancelAbility();
                    buyMenuGUI.SetActive(true);
                    TogglePlayerFunctions(false);
                }
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (placing && !isOpen || closing && !isOpen)
            {
                CancelAbility();
            }
        }//Cancel Placement

        if (placing && Input.GetMouseButtonDown(0))
        {
            placing = false;
            closing = true;
        }

        if(closing && Input.GetMouseButtonUp(0))
        {
            int tempMines = placeableMines;
            if (ammoBoxActive)
            {
                AmmoBoxPlace();
            }//If Ammo Box ability is active
            if (bombActive)
            {
                DropBomb();
            }
            if (healthPackActive)
            {
                HealthPackPlace();
            }
            if (minesActive)
            {
                MinesPlace();
            }
            if (weaknessZoneActive)
            {
                WeaknessZonePlace();
            }
            if (spikeTrapActive)
            {
                SpikeTrapPlace();
            }
            if (tauntTotemActive)
            {
                TauntTotemPlace();
            }
            //Checks to see if the ability was able to be used
            if (!ammoBoxActive && !bombActive && !healthPackActive &&!minesActive && !weaknessZoneActive && !spikeTrapActive && !tauntTotemActive)
            {
                TogglePlayerFunctions(true);
                closing = false;
            }
            else
            {
                if (minesActive)
                {
                    if (tempMines == placeableMines)
                    {
                        GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.enabled = true;
                        GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.text = "Cannot deploy this ability here.";
                    }
                }
                else
                {
                    GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.enabled = true;
                    GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.text = "Cannot deploy this ability here.";
                }
            }

        }//Does ability and reactivates player
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Used to elimate the crosshair dissapearing bug.
        {
            isOpen = false;
            buyMenuGUI.GetComponent<AMS_BuyControls>().notEnoughMessage.SetActive(false);
            buyMenuGUI.SetActive(false);
            TogglePlayerFunctions(true);
        }
    }

    public void SetUpForPlacing()
    {
        placing = true;
        closing = false;

        var cursor = GameObject.FindObjectOfType<WeaponCursorGUI>();
        if (cursor != null)
        {
            cursor.SetCursorStatus(1);
        }
        else { Debug.Log("Can't find main gui"); }
    }//this sets up the player buy menu for placing an object

    public void TogglePlayerFunctions(bool turnOn)
    {
        var gun = gameObject.GetComponent<AMS_GunManagement>();
        if (gun != null)
        {
            gun.reloading = false;
            gun.enabled = turnOn;
        }
        else { Debug.Log("gun not found"); }

        var laser = gameObject.GetComponentInChildren<LineRenderer>(true);
        if (laser != null) { laser.enabled = turnOn; }
        else { Debug.Log("laser not found"); }

        var cursor = GameObject.FindObjectOfType<WeaponCursorGUI>();
        if(cursor != null)
        {
            if (turnOn){ cursor.SetCursorStatus(2); }
            else { cursor.SetCursorStatus(0); }
        }
        else { Debug.Log("Can't find main gui"); }

    }//toggles player functions (gun and laser) to the state specified in the bool

    public void TogglePlayerFunctions(bool gunOn, bool laserOn, bool cursorOn)
    {
        var gun = gameObject.GetComponent<AMS_GunManagement>();
        if (gun != null)
        {
            gun.reloading = false;
            gun.enabled = gunOn;
        }
        else { Debug.Log("gun not found"); }

        var laser = gameObject.GetComponentInChildren<LineRenderer>(true);
        if (laser != null) { laser.enabled = laserOn; }
        else { Debug.Log("laser not found"); }

        var cursor = GameObject.FindObjectOfType<WeaponCursorGUI>();
        if (cursor != null)
        {
            if (cursorOn) { cursor.SetCursorStatus(2); }
            else { cursor.SetCursorStatus(0); }
        }
        else { Debug.Log("Can't find main gui"); }

    }//overload that lets you choose individual parts

    public void AmmoBoxPlace()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), ammoBox.transform.localScale * 1.5f, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "Default_Enemy" || hit.collider.gameObject.tag == "Enemy")
                {
                    GameObject ammoBoxTemp = Instantiate(ammoBox, new Vector3(hit2.point.x, hit2.point.y + 18, hit2.point.z), Quaternion.identity);
                    ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().type = ammoBoxType;
                    ammoBoxActive = false;
                    //Level Diffrences - FALL SPEED
                    if (ammoBoxType == "Default")
                    {
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[0] == 1)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 2;
                        }
                        //Level 2
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[0] == 2)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 4;
                        }
                        //Level 3
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[0] == 3)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 8;
                        }
                    }
                    if (ammoBoxType == "Unlock")
                    {
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[3] == 1)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 2;
                        }
                        //Level 2
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[3] == 2)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 4;
                        }
                        //Level 3
                        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[3] == 3)
                        {
                            ammoBoxTemp.GetComponent<AMS_DeployedHealthPack>().speed = 8;
                        }
                    }
                }
            }
        }
            
    }

    public void DropBomb()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), dropBomb.transform.localScale * 1.5f, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "Default_Enemy" || hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Resource" || hit.collider.gameObject.tag == "Target" || hit.collider.gameObject.GetComponent<AMS_PowerupPickup>() != null || hit.collider.gameObject.GetComponent<AMS_Ammo_Pickup>() != null || hit.collider.gameObject.GetComponent<AMS_HealthPickup>() != null || hit.collider.gameObject.GetComponent<AMS_SpikeTrap>() != null || hit.collider.gameObject.GetComponent<AMS_WeaknessZone>() != null || hit.collider.gameObject.GetComponent<TurretSwitchingScript>() != null)
                {
                    GameObject tempBomb = Instantiate(dropBomb, new Vector3(hit2.point.x, hit2.point.y + 18, hit2.point.z), Quaternion.identity);
                    bombActive = false;
                    GameObject tempExplosion = tempBomb.GetComponent<AMS_DroppedBomb>().explosionRadius;
                    //Level Diffrences  Greater Healing
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[2] == 1)
                    {
                        tempExplosion.transform.localScale = tempExplosion.transform.localScale;
                    }
                    //Level 2
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[2] == 2)
                    {
                        tempExplosion.transform.localScale = tempExplosion.transform.localScale * 2;
                    }
                    //Level 3
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[2] == 3)
                    {
                        tempExplosion.transform.localScale = tempExplosion.transform.localScale * 3;
                    }
                }
            }
        }           
    }

    public void HealthPackPlace()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), healthPack.transform.localScale * 1.5f, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "Default_Enemy" || hit.collider.gameObject.tag == "Enemy")
                {
                    GameObject tempHealthDrop = Instantiate(healthPack, new Vector3(hit2.point.x, hit2.point.y + 18, hit2.point.z), Quaternion.identity);
                    tempHealthDrop.GetComponent<AMS_DeployedHealthPack>().type = "Health";
                    healthPackActive = false;
                    //Level Diffrences  Greater Healing
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[1] == 1)
                    {
                        tempHealthDrop.GetComponent<AMS_DeployedHealthPack>().healAmount = 25;
                    }
                    //Level 2
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[1] == 2)
                    {
                        tempHealthDrop.GetComponent<AMS_DeployedHealthPack>().healAmount = 50;
                    }
                    //Level 3
                    if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[1] == 3)
                    {
                        tempHealthDrop.GetComponent<AMS_DeployedHealthPack>().healAmount = 100;
                    }
                }
            }
        }
    }

    public void MinesPlace()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), mine.transform.localScale, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    GameObject tempMine = Instantiate(mine, new Vector3(hit2.point.x, hit2.point.y, hit2.point.z), Quaternion.identity);
                    placeableMines--;
                    if (placeableMines == 0)
                    {
                        Debug.Log("Out of mines");
                        minesActive = false;
                    }
                }
            }
        }
    }

    public int MinesToPlace()
    {
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[4] == 1)
        {
            return 3;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[4] == 2)
        {
            return 5;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[4] == 3)
        {
            return 8;
        }
        return 3;
    }

    public void WeaknessZonePlace()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Floor")
            {
                GameObject tempWeakness = Instantiate(weaknessZone, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                tempWeakness.transform.localScale = new Vector3(GetWeaknessZoneSize(), .8f, GetWeaknessZoneSize());
                weaknessZoneActive = false;
            }
        }
    }

    public void SpikeTrapPlace()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), spikeTrap.transform.localScale * 1.5f, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor")
                {
                    Instantiate(spikeTrap, new Vector3(hit2.point.x, hit2.point.y, hit2.point.z), Quaternion.identity);
                    spikeTrapActive = false;
                }
            }
        }
    }

    public void TauntTotemPlace()
    {
        RaycastHit hit2;
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.BoxCast(new Vector3(hit2.point.x, 40, hit2.point.z), tauntTotem.transform.localScale * 1.5f, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "Default_Enemy" || hit.collider.gameObject.tag == "Enemy")
                {
                    GameObject tempTauntTotem = Instantiate(tauntTotem, new Vector3(hit2.point.x, hit2.point.y + 18, hit2.point.z), Quaternion.identity);
                    tempTauntTotem.GetComponent<AMS_DeployedTauntTotem>().healthOnDeploy = GetTauntTotemHealth();
                    tauntTotemActive = false;
                }
            }
        }
    }

    public float GetTauntTotemHealth()
    {
        //Level Diffrences  Greater Healing
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[8] == 1)
        {
            return 80;
        }
        //Level 2
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[8] == 2)
        {
            return 120;
        }
        //Level 3
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[8] == 3)
        {
            return 160;
        }
        return 80;
    }

    private int GetWeaknessZoneSize()
    {
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[6] == 1)
        {
            return 8;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[6] == 2)
        {
            return 12;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[6] == 3)
        {
            return 16;
        }
        return 8;
    }

    //Called by the buy menu
    public void UseAmmoBox()
    {
        cooldownControl.ammoBoxButton.SetActive(false);
        cooldownControl.currentammoBoxCooldown = cooldownControl.ammoBoxCooldown;
        SetUpForPlacing();
        ammoBoxActive = true;
        ammoBoxType = "Default";
        ActivateSound();
    }

    public void UseDropBomb()
    {
        cooldownControl.dropBombButton.SetActive(false);
        cooldownControl.currentDropBombCooldown = cooldownControl.dropBombCooldown;
        SetUpForPlacing();
        bombActive = true;
        ActivateSound();
    }

    public void UseHealthPack()
    {
        cooldownControl.healthPackButton.SetActive(false);
        cooldownControl.currenthealthPackCooldown = cooldownControl.healthPackCooldown;
        SetUpForPlacing();
        healthPackActive = true;
        ActivateSound();
    }

    public void UseUnlockAmmo()
    {
        cooldownControl.unlockAmmoButton.SetActive(false);
        cooldownControl.currentUnlockAmmoCooldown = cooldownControl.unlockAmmoCooldown;
        SetUpForPlacing();
        ammoBoxActive = true;
        ammoBoxType = "Unlock";
        ActivateSound();
    }

    public void UseMines()
    {
        cooldownControl.minesControlButton.SetActive(false);
        cooldownControl.currentminesCooldown = cooldownControl.minesCooldown;
        placeableMines = MinesToPlace();
        SetUpForPlacing();
        minesActive = true;
        ActivateSound();
    }

    public void UseBaseHealthRestore()
    {
        cooldownControl.baseRestoreButton.SetActive(false);
        cooldownControl.currentBaseRestoreCooldown = cooldownControl.baseRestoreCooldown;
        baseHealthScript = FindObjectOfType<BaseHealth>().GetComponent<BaseHealth>();
        baseHealthScript.currentHP += baseHealthScript.maxHP / GetRestoreAmount();
        if (baseHealthScript.currentHP > baseHealthScript.maxHP)
        {
            baseHealthScript.currentHP = baseHealthScript.maxHP;
        }
        if (!ammoBoxActive && !healthPackActive && !bombActive && !minesActive)
        {
            TogglePlayerFunctions(true);
        }
        ActivateSound();
    }

    public void UseWeaknessZone()
    {
        cooldownControl.weaknessZoneButton.SetActive(false);
        cooldownControl.currentWeaknessZoneCooldown = cooldownControl.weaknessZoneCooldown;
        SetUpForPlacing();
        weaknessZoneActive = true;
        ActivateSound();
    }

    public void UseSpikeTrap()
    {
        cooldownControl.spikeTrapButton.SetActive(false);
        cooldownControl.currentSpikeTrapCooldown = cooldownControl.spikeTrapCooldown;
        SetUpForPlacing();
        spikeTrapActive = true;
        ActivateSound();
    }

    public void UseTauntTotem()
    {
        cooldownControl.tauntToemButton.SetActive(false);
        cooldownControl.currentTauntTotemCooldown = cooldownControl.tauntTotemCooldown;
        SetUpForPlacing();
        tauntTotemActive = true;
        ActivateSound();
    }

    private float GetRestoreAmount()
    {
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[5] == 1)
        {
            return 5;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[5] == 2)
        {
            return 4;
        }
        if (buyMenuGUI.GetComponent<AMS_BuyControls>().levelInts[5] == 3)
        {
            return 3;
        }
        //No value
        Debug.Log("No level");
        return 0;
    }

    public void ActivateAbility(int ability)
    {
        if (cooldownControl.purchasableAbilities[ability] == true)
        {
            //Checks to see if another ability is already active
            if (!ammoBoxActive && !bombActive && !healthPackActive && !minesActive && !spikeTrapActive && !weaknessZoneActive && !spikeTrapActive && !tauntTotemActive)
            {
                //Ammo Box
                if (ability == 0)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseAmmoBox();
                }
                //Health Pack
                if (ability == 1)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseHealthPack();
                }
                //Bomb
                if (ability == 2)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseDropBomb();
                }
                //Unlock Ammo
                if (ability == 3)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseUnlockAmmo();
                }
                //Mine
                if (ability == 4)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseMines();
                }
                //Base health restore
                if (ability == 5)
                {
                    UseBaseHealthRestore();
                }
                //Weakness Zone 
                if (ability == 6)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseWeaknessZone();
                }
                //Spike Trap
                if (ability == 7)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseSpikeTrap();
                }
                //Taunt Totem
                if (ability == 8)
                {
                    TogglePlayerFunctions(false, false, true);
                    UseTauntTotem();
                }
                cooldownControl.purchasableAbilities[ability] = false;
            }
            else
            {
                //Display message saying player has a current dropable ability already active
                Debug.Log("Ability already active");
                GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.enabled = true;
                GameObject.FindObjectOfType<AMS_GUIUpdater>().doubleAbilityText.text = "Must Deploy Current Abillity Before Selecting A New One.";
            }
            
        }
    }

    public void CancelAbility()
    {
        Debug.Log("Cancel Ability");
        placing = false;
        TogglePlayerFunctions(true);
        //Ammo and unlock ammo
        if (ammoBoxActive)
        {
            if (ammoBoxType == "Default")
            {
                cooldownControl.currentammoBoxCooldown = 0;
                cooldownControl.purchasableAbilities[0] = true;
            }
            if (ammoBoxType == "Unlock")
            {
                cooldownControl.currentUnlockAmmoCooldown = 0;
                cooldownControl.purchasableAbilities[3] = true;
            }
            ammoBoxActive = false;
        }
        //Healthpack
        if (healthPackActive)
        {
            cooldownControl.currenthealthPackCooldown = 0;
            cooldownControl.purchasableAbilities[1] = true;
            healthPackActive = false;
        }
        //Bomb
        if (bombActive)
        {
            cooldownControl.currentDropBombCooldown = 0;
            cooldownControl.purchasableAbilities[2] = true;
            bombActive = false;
        }
        //Mine
        if (minesActive)
        {
            if (placeableMines == MinesToPlace())
            {
                cooldownControl.currentminesCooldown = 0;
                cooldownControl.purchasableAbilities[4] = true;
            }
            minesActive = false;
            //Debug.Log(minesActive);
        }
        //Weakness Zone
        if (weaknessZoneActive)
        {
            cooldownControl.currentWeaknessZoneCooldown = 0;
            cooldownControl.purchasableAbilities[6] = true;
            weaknessZoneActive = false;
        }
        //SpikeTrap
        if (spikeTrapActive)
        {
            cooldownControl.currentSpikeTrapCooldown = 0;
            cooldownControl.purchasableAbilities[7] = true;
            spikeTrapActive = false;
        }
        //Taunt Totem
        if (tauntTotemActive)
        {
            cooldownControl.currentTauntTotemCooldown = 0;
            cooldownControl.purchasableAbilities[8] = true;
            tauntTotemActive = false;
        }
        CancelSound();
    }

    private void ActivateSound()
    {
        if(GetComponent<AudioSwitcherScript>() != null)
        {
            if(GetComponent<AudioSwitcherScript>().CheckIfSoundIsPlaying(activateAbilitySound))
            {
                GetComponent<AudioSwitcherScript>().KillSound(activateAbilitySound);
            }
            GetComponent<AudioSwitcherScript>().PlaySound(activateAbilitySound);
        }
    }

    private void CancelSound()
    {
        if (GetComponent<AudioSwitcherScript>() != null)
        {
            if (GetComponent<AudioSwitcherScript>().CheckIfSoundIsPlaying(activateAbilitySound))
            {
                GetComponent<AudioSwitcherScript>().KillSound(activateAbilitySound);
            }
        }
    }
}

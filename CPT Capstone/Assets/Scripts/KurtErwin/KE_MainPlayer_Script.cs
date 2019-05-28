using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameController;
using UnityEngine.UI;

public class KE_MainPlayer_Script : MonoBehaviour
{
    public float grav;
    public float runSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float currentStamina = 0;
    public float MaxStamina = 5;
   // public bool IsSprinting = false;
    private float currentSpeed;
    public bool playerIsActive = true;

    private KeyCode grabResourceKey = KeyCode.E;
    public float grabRange = 2f;
    [HideInInspector]
    public bool carrying { get; private set; }
    public GameObject carriedObj;
    private float carriedOffsetY;
    private AudioSwitcherScript aSwitcher;
    public AudioClip grabResourceSound;
    public AudioClip releaseResourceSound;
    //AMS Addition 
    public GameObject laser;
    // public bool playerLoad; //tells the player to load data (if save is available)
    // public bool loadAgain;

    //Holds the upgrade values
    [HideInInspector]
    public int[] passiveUpgradeLevels = { 1, 1, 1, 1, 1 };

    private CharacterController charCont;
    private Vector3 heading = new Vector3(0,0,0);
    public Image staminaBarDisplay;
    public Text staminaBarText;
    public float staminaMaxFill = 1.0f;
    public float staminaTimer;
    public bool handy = true;

    void Start()
    {
        carrying = false;
        carriedObj = null;
        charCont = GetComponent<CharacterController>();
        currentSpeed = runSpeed;
        aSwitcher = gameObject.GetComponent<AudioSwitcherScript>();
        //loadAgain = true;
        if(aSwitcher == null) { Debug.Log("player object does not have a audio switcher script"); }

        if (StaticCheckpointController.shouldTrigger)
        {
            WarpTo(StaticCheckpointController.WhereToSpawn());
        }
        currentStamina = MaxStamina;
        sprintSpeed = runSpeed;
        staminaBarDisplay = GameObject.Find("StamBar").GetComponent<Image>();
        staminaBarDisplay.gameObject.SetActive(false);
        staminaBarDisplay.fillAmount = staminaMaxFill;
        staminaBarText = GameObject.Find("StamBarText").GetComponent<Text>();
        staminaBarText.enabled = false;
        
    }

    void Update()
    {       
        if (playerIsActive)//to allow for killing the player controls
        {
            AMS_ScoreController.decreaseMultiplier();
            currentSpeed = runSpeed; //this is the default movement speed

            if (Input.GetKeyDown(grabResourceKey) && handy)
            {
                if(carriedObj == null)
                {
                    var dragables = FindObjectsOfType<DragScript>();
                    if (dragables.Length > 0)
                    {
                        var closestRange = Mathf.Infinity;
                        DragScript closestDragable = null;
                        foreach (DragScript dragable in dragables)
                        {
                            var dis = Vector3.Distance(transform.position, dragable.transform.position);
                            if (dis < closestRange && dis < grabRange) { closestRange = dis; closestDragable = dragable; }
                        }//find nearest in grab range

                        if(closestDragable != null)
                        {
                            SetCarrying(closestDragable.gameObject);
                        }//there is one to grab
                        else { }//none to grab

                    }//are there dragables in the scene
                }//wanting to pick up
                else
                {
                    SetCarrying(null);
                }//wanting to drop
            }//player is trying to grab or drop maybe

            var gun = GetComponent<AMS_GunManagement>();
            if (gun != null)
            {
                if (gun.reloading || gun.IsFiring())
                { currentSpeed = walkSpeed; }
                else
                {
                    currentSpeed = runSpeed;
                    PlayerSprint();
                   
                }
            }
            else { Debug.Log("no gun found for movement calc"); }
            
            if (charCont.isGrounded)
            {
                heading = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                heading = heading.normalized * (currentSpeed * Time.deltaTime);
            }//only allow controls when player is grounded

            heading.y -= grav * Time.deltaTime;

            charCont.Move(heading);
        }
    }

    private void LateUpdate()
    {
        if (carriedObj != null)
        {
            var carriedPos = transform.position + (transform.forward * -1.1f) + (transform.up * .5f);
            carriedPos.y += carriedOffsetY;
            carriedObj.transform.position = carriedPos;
        }//move object with player
        else if (carrying) { SetCarrying(null); }
    }

    public void WarpTo(Vector3 location)
    {
        charCont.enabled = false;
        gameObject.transform.position = location;
        charCont.enabled = true;
    }

    public void SetCarrying(GameObject itemToCarry)
    {
        bool isCarry;
        if (itemToCarry == null)
        {
            isCarry = false;
            if(carriedObj != null)
            {
                var script = carriedObj.GetComponent<DragScript>();
                if(script != null)
                {
                    script.grounded = false;
                    //Debug.Log("how often do I do this?");
                    SwitchColliders(true, carriedObj);
                }
            }//but we have something to put down
        }//we aren't picking anything up
        else
        {
            isCarry = true;
            SwitchColliders(false, itemToCarry);
            carriedOffsetY = itemToCarry.transform.position.y - transform.position.y;
        }//we are picking something up

        carriedObj = itemToCarry;
        carrying = isCarry;
        //AMS Addition
        laser.SetActive(!isCarry);
        var gun = GetComponent<AMS_GunManagement>();
        if (gun != null)
        {
            gun.ToggleGun(!isCarry);
        }
        else { Debug.Log("no gun found"); }

        if (isCarry) { aSwitcher.PlaySound(grabResourceSound); }
        else { aSwitcher.PlaySound(releaseResourceSound); }
    }
     
    public void PlayerSprint()
    {
        var Sprinting = (Input.GetKey(KeyCode.LeftShift));
        staminaTimer = MaxStamina;
        //if (IsSprinting)
        {
            if(!Sprinting && currentStamina < MaxStamina)
            {
                staminaBarText.enabled = true;
                currentSpeed = runSpeed;
                currentStamina += Time.deltaTime;
                staminaBarDisplay.fillAmount += staminaMaxFill / staminaTimer * Time.deltaTime;
                if (currentStamina >= MaxStamina)
                {
                    staminaBarDisplay.gameObject.SetActive(false);
                    staminaBarText.enabled = false;
                }
                //Debug.Log("The Player is resting stamina level = " + currentStamina);

            }
            else
            {
                if(Sprinting && currentStamina > 0)
                {
                    //  IsSprinting = true;
                    // currentSpeed = Mathf.SmoothStep(currentSpeed, sprintSpeed, MaxStamina);
                    //currentSpeed += sprintSpeed;
                    staminaBarText.enabled = true; 
                    staminaBarDisplay.gameObject.SetActive(true);
                    currentSpeed += sprintSpeed;
                    currentStamina -= Time.deltaTime;
                    staminaBarDisplay.fillAmount -= staminaMaxFill / staminaTimer * Time.deltaTime;
                    if (currentStamina <= 0)
                    {
                    //    Debug.Log("The Player ran out of stamina");
                        currentSpeed = runSpeed;
                        currentStamina = 0;
                    }
                }
            }
        }
      //  Debug.Log("The Player's Current Stamina is " + currentStamina);
      //  Debug.Log("The Stamina Bar's fill amount is " + staminaBarDisplay.fillAmount);  
     //   Debug.Log("The Player's Current Speed is " + currentSpeed);
      
    }

    private void SwitchColliders(bool state, GameObject parent)
    {
        var colliders = parent.GetComponentsInChildren<Collider>();
        if(colliders.Length > 0)
        {
            foreach(Collider collider in colliders)
            {
                collider.enabled = state;
            }
        }
    }

    /* Commented out, works mostly. Better fix was made.
    public void SavePlayer()
    {
        GameController.Save(this);        
    }

    public void WaitLoadPlayer()
    {
        PlayerData data = GameController.LoadPlayer();
        if (data != null)
        {
            playerLoad = data.playerLoad;
            if (playerLoad == true && loadAgain == true)
            {
                Debug.Log("loading player");
                LoadPlayer();
                playerLoad = false;
                loadAgain = false;
                data.playerLoad = false;
                FindObjectOfType<loadme>().initialLoad = false;
            }
        }
    }

    public void LoadPlayer()
    {
        
        //sets variables for each component for the player stats (health, ammo, unlocked guns, position)
        var health = GetComponent<AMS_Health_Management>();
        var ammo1 = GetComponent<AMS_GunManagement>();
        var ammo2 = GetComponent<AMS_GunManagement>();
        var unlocked = GetComponent<AMS_GunManagement>();
        var ammoClips1 = GetComponent<AMS_GunManagement>();
        var ammoClips2 = GetComponent<AMS_GunManagement>();
        var curLevel = GetComponent<MainMenu>();
        //var bought = GetComponent<AMS_BuyControls>();
        //var resources = FindObjectOfType<AMS_ResourceController>();
        var altFireAmmo = GetComponent<AMS_GunManagement>();
        var currentGun = GetComponent<AMS_GunManagement>();


        PlayerData data = GameController.LoadPlayer();

        //Left side is variables being replaced. Right side is saved variables replacing the left.
        health.currentHealth = data.health;
        ammoClips1.currentClips = data.ammoClips1;
        ammoClips2.gun2currentClips = data.ammoClips2;
        ammo1.currentAmmo = data.ammo1;
        ammo2.gun2currentAmmo = data.ammo2;
        unlocked.unlocked = data.unlocked;
        //bought.boughtWeapon = data.boughtweapon;
        //resources.currentResources = data.resources;
        altFireAmmo.defaultAlternateFireAmmo = data.altFireammo;
        currentGun.currentGun = data.currentGun;

        Vector3 position = new Vector3(data.spawn[0], data.spawn[1], data.spawn[2]);
        transform.position = position;

    }

    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        GameController.Save(this);
    }*/
}




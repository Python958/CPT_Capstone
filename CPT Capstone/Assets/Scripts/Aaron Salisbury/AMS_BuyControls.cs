using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AMS_BuyControls : MonoBehaviour
{
    private AMS_ResourceController resourceControls;
    private AMS_BuyMenuPlayer buyMenuPlayer;
    private AMS_GunManagement playerGun;
    public GameObject notEnoughMessage;
    public AudioClip boughtSound;
    public bool playerLoad;

    //Used to switch Unlock to Ammo
    public GameObject mysteryUnlock;
    public GameObject mysteryUnlockButton;
    public GameObject unlockAmmo;
    public Text unlockAmmoText;
    public string boughtWeapon = null;

    //Bullets for weapon unlocks
    public GameObject[] WeaponChoices;
    public GameObject railGunBullet;
    public GameObject shotGunBullet;
    public GameObject ricochetRifleBullet;

    public Text BuyMenuDescriptionText;

    //Cooldown Controller
    public AMS_BuyMenuCooldownController cooldownControl;

    // Levelup System
    //public int ammoBoxLevel = 1;
    //public int bombDropLevel = 1;
    //public int healthPackLevel = 1;
    //public int unlockAmmoLevel = 1;

    //Abilities
    public Text[] levelTextArray;
    public Text[] levelDescriptionArray;
    public int[] levelInts;
    public int[] levelCosts;

    //Upgrades
    public int[] passiveLevelInts;
    public int[] passiveLevelCosts;
    public Text[] passiveLevelText;
    public GameObject[] passiveUpgradeButton;

    private bool switchOnce = false;
    //Used to hold the unlock purchase costs
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine, base restore, weakness zone, spike trap")]
    public int[] abilityCosts;
    //Grey box images that displays if an ability is not purchased
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine, base restore, weakness zone, spike trap")]
    public GameObject[] greyBoxes;
    //Buttons to unlock the ability
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine, base restore, weakness zone, spike trap")]
    public GameObject[] unlockButtons;
    //Buttons to trigger the ability upgrade
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine, base restore, weakness zone, spike trap")]
    public GameObject[] upgradeButtons;
    //Buttons to trigger the use function
    [Tooltip("Order Ammo, health, bomb, unlock ammo, mine, base restore, weakness zone, spike trap")]
    public GameObject[] useButtons;

    public GameObject[] abilityContainers;

    public int[] abilitiesToDisableOnStart;

    private string[] descriptionText = {
        
        //No description 0
        "",
                        //Abilities
        //Ammo Box 1
        "Add an additional 5 Clips to your weapon.",
        //Health Kit 2
        "An Elite Tactical First Aid Trauma Kit.",
        //Drop Bomb 3
        "A B61 Nuclear Bomb with Proximity Radar, explodes on contact.",
        //Unlock Weapon 4
        "Purchase a Secondary Weapon, this may save your life when you can't reload!",
        //Shotgun 5
        "Fires multiple projectiles however range is extremely short.",
        //Rail Gun 6
        "Long range, high damage, slow firing speed, small clip.",
        //Ricochet Rifle 7
        "Fires alot of projectiles that bounce around the enviroment.",
        //Unlock Ammo 8
        "Deploy an ammo box that refills your unlocked ammo.",
        //Placeable Mines 9
        "A Placeable Explosive that detonates when an Enemy walks over it.",
        //Base Restore 10
        "A quick restore of your base's health. This has a long cooldown so use it wisely.",
        //Weakness Zone 11
        "A zone where the enemie's health is cut in half while inside.",
        //Spike Trap 12
        "A trap that you can deploy. Be careful because it can hurt you too.",
        //Taunt Totem 13
        "A totem you can deploy that will enrage your eneimes causing them to go after it.",
        //Last buy menu ability 14
        "",
                        
                        //Upgrades
        //Ammo Box 15
        "Increased Deployment Speed",
        //Health Kit 16
        "Increased Health Received",
        //Drop Bomb 17
        "Larger Explosive Radius",
        //Unlock Ammo 18
        "Increased Deployment Speed",
        //Placeable Mines 19
        "Increased Mines Per Use",
        //Base Restore 20
        "Increased Health Restored",
        //Weakness Zone 21
        "Increased Area Of Effect",
        //Spike Trap 22
        "Reduced Damage To Player",
        //Taunt Totem 23
        "Increased Totem's Health",
        //Last buy menu ability 24
        "",
                        
                        //Passives
        //Max Health 25
        "Increases the player's max health.",
        //Base Turret 26
        "Increases the effectiveness of the turrets attached to the base.",
        //Reload Speed 27
        "Increases the reload speed of all guns.",
        //Clip Size 28
        "Increases the clip size of the default weapon.",
        //Powerup Duration 29
        "Extends the duration of powerups.",

        //Blank
        ""
        };

    //Order Ammo, health, bomb, unlock ammo 
    //If true it is enabled if false it needs to be unlocked
    public bool[] unlockedAbilities;



    // Start is called before the first frame update
    void Start()
    {
        resourceControls = GameObject.FindObjectOfType<AMS_ResourceController>();
        if(resourceControls == null) { Debug.Log("can't find AMS_ResourceController"); }
        buyMenuPlayer = GameObject.FindObjectOfType<AMS_BuyMenuPlayer>();
        playerGun = GameObject.FindObjectOfType<AMS_GunManagement>();
        BuyMenuDescriptionText.enabled = false; //disable buy menu description
        AbilitiesToDisableOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        updateLevelGUI();
        updateUpgradeText();
        SwitchUnlockToAmmo();
        GreyedOutButtons();
        //Debug.Log("Hey its open");
    }

    public void BuyWeaponUnlock()
    {
        //Can buy
        if (resourceControls.currentResources >= 40)
        {
            resourceControls.currentResources -= 40;
            foreach (GameObject choice in WeaponChoices)
            {
                choice.SetActive(true);
                mysteryUnlockButton.SetActive(false);
            }
        }
        else
        {
            notEnoughMessage.SetActive(true);
        }

    }

    public void SelectWeapon(int choice)
    {
        if (choice == 1)
        {
            boughtWeapon = "Shotgun";
            playerGun.UnlockWeapon("Shotgun");
        }
        if (choice == 2)
        {
            boughtWeapon = "Rail Gun";
            playerGun.UnlockWeapon("Rail Gun");
        }
        if (choice == 3)
        {
            boughtWeapon = "Ricochet Rifle";
            playerGun.UnlockWeapon("Ricochet Rifle");
        }
        buyMenuPlayer.gameObject.GetComponent<AudioSwitcherScript>().PlaySound(boughtSound);
        buyMenuPlayer.TogglePlayerFunctions(true);
        CloseMenu(true);
    }

    public void CloseMenu(bool fullyActivatePlayer)
    {
        buyMenuPlayer.isOpen = false;
        notEnoughMessage.SetActive(false);
        gameObject.SetActive(false);
        buyMenuPlayer.TogglePlayerFunctions(fullyActivatePlayer);
        BuyMenuDescriptionText.GetComponent<Text>().text = null;
    }

    private void SwitchUnlockToAmmo()
    {
        if (!switchOnce)
        {
            if (boughtWeapon == "Shotgun" || boughtWeapon == "Rail Gun" || boughtWeapon == "Ricochet Rifle" || boughtWeapon == "Unlock Weapon")
            {
                mysteryUnlock.SetActive(false);
                unlockAmmo.SetActive(true);
                unlockAmmoText.text = boughtWeapon + " Ammo";
                switchOnce = true;
            }
        }
    }

    public void mouseoverDescription(int ability)
    {
        BuyMenuDescriptionText.enabled = true;
        BuyMenuDescriptionText.GetComponent<Text>().text = descriptionText[ability];
    }

    public void mouseExitAllButtons()
    {
        BuyMenuDescriptionText.enabled = false; //disable buy menu description
    }

    private void updateLevelGUI()
    {
        for (int i = 0; i < levelInts.Length; i++)
        {
            //See if text needs to be updated
            if (levelTextArray[i].text != "LVL: " + levelInts[i])
            {
                levelTextArray[i].text = "LVL: " + levelInts[i];
                //Updates description;
                if (levelInts[i] == 2)
                {
                    //Turns on the disabled descriptions
                    levelDescriptionArray[i].enabled = true;
                }
                else
                {
                    //Add an extra +
                    levelDescriptionArray[i].text = levelDescriptionArray[i].text + "+";
                }
            }
        }
        for (int i = 0; i < passiveLevelInts.Length; i++)
        {
            //See if text needs to be updated
            if (passiveLevelText[i].text != "LVL: " + passiveLevelInts[i])
            {
                passiveLevelText[i].text = "LVL: " + passiveLevelInts[i];
            }
        }
    }

    public void levelUpAbility(int ability)
    {
        //if not level 3
        if (levelInts[ability] != 3)
        {
            if (levelInts[ability] == 2)
            {
                if (resourceControls.currentResources >= levelCosts[(ability * 2) + 1])
                {
                    resourceControls.currentResources -= levelCosts[(ability * 2) + 1];
                    levelInts[ability]++;
                }
                else
                {
                    notEnoughMessage.SetActive(true);
                }
            }
            if (levelInts[ability] == 1)
            {
                if(resourceControls.currentResources >= levelCosts[ability * 2])
                {
                    resourceControls.currentResources -= levelCosts[ability * 2];
                    levelInts[ability]++;
                }
                else
                {
                    notEnoughMessage.SetActive(true);
                }
            }
        }
    }

    public void levelUpPassive(int upgrade)
    {
        //if not level 3
        //I changed the below from levelInts to passiveLevelInts as that seems what it should be but I'm not sure this could ever fail anyway. -KE
        if (passiveLevelInts[upgrade] != 4)
        {
            if (passiveLevelInts[upgrade] == 3)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[(upgrade * 3) + 2])
                {
                    resourceControls.currentResources -= passiveLevelCosts[(upgrade * 3) + 2];
                    passiveLevelInts[upgrade]++;
                    passiveUpgradeButton[upgrade].SetActive(false);
                }
                else
                {
                    notEnoughMessage.SetActive(true);
                }
            }
            if (passiveLevelInts[upgrade] == 2)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[(upgrade * 3) + 1])
                {
                    resourceControls.currentResources -= passiveLevelCosts[(upgrade * 3) + 1];
                    passiveLevelInts[upgrade]++;
                }
                else
                {
                    notEnoughMessage.SetActive(true);
                }
            }
            if (passiveLevelInts[upgrade] == 1)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[upgrade * 3])
                {
                    resourceControls.currentResources -= passiveLevelCosts[upgrade * 3];
                    passiveLevelInts[upgrade]++;
                }
                else
                {
                    notEnoughMessage.SetActive(true);
                }
            }
        }
        FindObjectOfType<KE_MainPlayer_Script>().passiveUpgradeLevels = passiveLevelInts;
    }

    //New Unlock functions
    public void UnlockAbility(int ability)
    {
        //Check to see if they can buy it
        if (resourceControls.currentResources >= abilityCosts[ability])
        {
            //Turn off the greyed out box
            greyBoxes[ability].SetActive(false);
            //Turn on the upgrade button
            upgradeButtons[ability].SetActive(true);
            //Turn on the use button
            useButtons[ability].SetActive(true);
            //Turn off the unlock button 
            unlockButtons[ability].SetActive(false);
            //Takes the cost
            resourceControls.currentResources -= abilityCosts[ability];
            //Sets the unlocked variable to true
            unlockedAbilities[ability] = true;
            cooldownControl.purchasableAbilities[ability] = true;
        }
        else
        {
            notEnoughMessage.SetActive(true);
        }
    }

    public void GreyedOutButtons()
    {
        //Unlock Abilities
        for (int i = 0; i < unlockButtons.Length; i++)
        {
            if (resourceControls.currentResources >= abilityCosts[i])
            {
                unlockButtons[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                unlockButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        //Ability Upgrades
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (levelInts[i] == 2)
            {
                if (resourceControls.currentResources >= levelCosts[(i * 2) + 1])
                {
                    upgradeButtons[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    upgradeButtons[i].GetComponent<Button>().interactable = false;
                }
            }
            if (levelInts[i] == 1)
            {
                if (resourceControls.currentResources >= levelCosts[(i * 2)])
                {
                    upgradeButtons[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    upgradeButtons[i].GetComponent<Button>().interactable = false;
                }
            }
        }
        //Passive Upgrades
        for (int i = 0; i < passiveUpgradeButton.Length; i++)
        {
            if (passiveLevelInts[i] == 3)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[(i * 3) + 2])
                {
                    passiveUpgradeButton[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    passiveUpgradeButton[i].GetComponent<Button>().interactable = false;
                }
            }
            if (passiveLevelInts[i] == 2)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[(i * 3) + 1])
                {
                   passiveUpgradeButton[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    passiveUpgradeButton[i].GetComponent<Button>().interactable = false;
                }
            }
            if (passiveLevelInts[i] == 1)
            {
                if (resourceControls.currentResources >= passiveLevelCosts[(i * 3)])
                {
                    passiveUpgradeButton[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    passiveUpgradeButton[i].GetComponent<Button>().interactable = false;
                }
            }
        }
        //Weapon Unlock
        if (resourceControls.currentResources >= 40)
        {
            mysteryUnlockButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            mysteryUnlockButton.GetComponent<Button>().interactable = false;
        }
    }

    //mainly done so I can make it zero cost
    public void UnlockAbility(int ability, int cost)
    {
        //Check to see if they can buy it
        if(resourceControls == null) { Debug.Log("where did resource Controls go"); resourceControls = FindObjectOfType<AMS_ResourceController>(); }
        if (resourceControls.currentResources >= cost)
        {
            //Turn off the greyed out box
            greyBoxes[ability].SetActive(false);
            //Turn on the upgrade button
            Debug.Log(upgradeButtons[ability]);
            upgradeButtons[ability].SetActive(true);
            //Turn on the use button
            useButtons[ability].SetActive(true);
            //Turn off the unlock button 
            unlockButtons[ability].SetActive(false);
            //Takes the cost
            resourceControls.currentResources -= cost;
            //Sets the unlocked variable to true
            unlockedAbilities[ability] = true;
            cooldownControl.purchasableAbilities[ability] = true;
        }
        else
        {
            notEnoughMessage.SetActive(true);
        }
    }//overload for main UnlockAbility function

    private void updateUpgradeText()
    {
        for(int i = 0; i <= upgradeButtons.Length - 1 ; i++)
        {
            if (levelInts[i] == 1)
            {
                upgradeButtons[i].GetComponentInChildren<Text>().text = "Upgrade: " + levelCosts[i * 2];
            }
            if (levelInts[i] == 2)
            {
                upgradeButtons[i].GetComponentInChildren<Text>().text = "Upgrade: " + levelCosts[(i * 2 + 1)];
            }
            if (levelInts[i] == 3)
            {
                upgradeButtons[i].SetActive(false);
            }
        }
        for (int i = 0; i <= passiveUpgradeButton.Length - 1; i++)
        {
            if (passiveLevelInts[i] == 1)
            {
                passiveUpgradeButton[i].GetComponentInChildren<Text>().text = "Upgrade: " + passiveLevelCosts[i * 3];
            }
            if (passiveLevelInts[i] == 2)
            {
                passiveUpgradeButton[i].GetComponentInChildren<Text>().text = "Upgrade: " + passiveLevelCosts[(i * 3 + 1)];
            }
            if (passiveLevelInts[i] == 3)
            {
                passiveUpgradeButton[i].GetComponentInChildren<Text>().text = "Upgrade: " + passiveLevelCosts[(i * 3 + 2)];
            }
            if (passiveLevelInts[i] == 4)
            {
                passiveUpgradeButton[i].SetActive(false);
            }
        }
    }

    public void UseAbility(int ability)
    {
        buyMenuPlayer.ActivateAbility(ability);

        //the exception is here because there is no means of reactivating cursor if the player buys a base restore
        //the current set up assumes all abilities are not instant and base restore is
        if (ability != 5) CloseMenu(false);
        else CloseMenu(true);
    }

    private void AbilitiesToDisableOnStart()
    {
        foreach (int abilityNumber in abilitiesToDisableOnStart)
        {
            abilityContainers[abilityNumber].SetActive(false);
        }
    }

    /*  void WaitPlayerLoad()
      {
          PlayerData data = GameController.LoadPlayer();
          if (data != null)
          {
              playerLoad = data.playerLoad;
              if (playerLoad == true)
              {
                  LoadPlayer();
                  playerLoad = false;
                  data.playerLoad = false;
              }
          }
      }
      public void LoadPlayer()
      {
          PlayerData data = GameController.LoadPlayer();
          boughtWeapon = data.boughtweapon;
 data.boughtweapon;
    }*/
}

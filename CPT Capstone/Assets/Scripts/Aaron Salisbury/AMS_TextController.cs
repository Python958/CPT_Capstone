using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_TextController : MonoBehaviour
{
    public Text textBox;
    public int currentDisplayText = 0;
    private string[] displayText = new string[] { "dsd", "sdsd", "dsds" };
    public AudioClip[] audioTracks;
    public AudioClip[] interuptNoise;
    private AudioSource myAudioSource;
    public GameObject[] tutorialDoors;
    public GameObject[] tutorialTargets;
    private GameObject player;
    private int tempNumber;

    public GameObject[] buyMenuOptions;
    private AMS_GunManagement playerGun;
    private AMS_BuyMenuPlayer buyPlayer;
    public AMS_BuyControls buyMenu;
    public GameObject[] textUpdaters;

    public GameObject[] weaponUnlockButtonsToTurnOff;
    public GameObject dropBombUseButton;

    public GameObject[] passiveUpgrades;

    public Collider levelGate;

    public GameObject dragableTurret;
    public GameObject[] explosiveBarrelSpawner;

    public GameObject[] practiceWaveComponets;

    public GameObject[] abilityUpgradeButtons;
    private bool upgradeActive = false;

    public GameObject turretBuffer;

    private bool once = false;
    private bool once2 = false;
    private int tempClips = 0;

    private float clipHasBeenPlayingFor = 0;
    private float interupttimer = 0;

    private AMS_TutorialPointer arrow;

    private float timeSpent = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateText());
        //Put all text in this string to make a new text just put , between the "". 
        displayText = new string[] {
            //0
            "Listen up you lawn dart, my name is General Gnome, in order for you to stand any chance out in the garden you better listen to my instruction. First, put those legs to use by using the WASD keys.",
            //1
            "Now that you have proven you have a brain in that cone head of yours, I am trusting you with every gnome’s best friend an AK47. Now point that gun at that duck and press the left mouse button to hear him make his last quack.",
            //2
            "Now give in to your instincts and find one of those delectable mushrooms.",
            //3
            "This is going to go against every one of your instincts but don’t eat that mushroom instead drag it behind you with the E button.",
            //4
            "Okay now run back to your shack with the Shift key. This will transport the mushroom directly to me, and in return, I will assist you. Remember while dragging objects you won’t be able to fire your gun. Note to self-figure... out how to fire a gun without using your hands.",
            //5
            "That mushroom was on the small side, so it is only worth let’s say 50 resources. Now open the buy menu with B. Then use 10 of those resources to purchase access to ammo boxes. That means click Unlock: 10. Now click the Use button that appeared.",
            //6
            "Now just left click wherever you want me to deploy that ammo box.",
            //7
            "Since it takes forever for that ammo box to land let's go over reloading. I can’t believe I have to explain this, but you reload by pressing R. Okay, I guess this is a little interesting press R while the reload is halfway through and it will be completed instantly. How that works, I have no idea.",
            //8
            "Now that that ammo box has finally shown up it's time to collect. Walk over and press E to get your used clips back. If you take your sweet gnome time, I am going to take the box away so don’t test my patients.",
            //New 9
            "Now take back part of the garden by opening the fence to the north. You do this by walking up to it and pressing the E button. This will cost you some resources, but it will push the garden animals out of your territory.",
            //10
            "You dumb lawn dart, you didn’t have enough resources to open that fence. I just gave you a bonus, notice how the fence is now green that means you have enough. Well what are you waiting for open it up already, you dumb mushroom loving simpleton.",
            //11
            "With the leftover resources from your bonus it's time to nuke these bird-brained foes. Open up the buy menu with B and click unlock 40. Once that is done proceed to click use.",
            //12
            "Now for my favorite part of the lesson, drop the nuke on those bad eggs and see them wish they never hatched. Stand back from the explosion we have lost a lot of dumb recruits to our own bombs and I hate filling out the paperwork.",
            //13
            "YES!!! No extra paperwork tonight. …. Ohh yea, proceed to the next area.",
            //14
            "Now that you have proven that you won’t immediately kill yourself with explosives, you now have access to the grenade launcher attachment on your gun. Fire a round at that dumb duck by pressing either space or the middle mouse button.",
            //15
            "Let’s get the Gnominator, aka the pump action shotgun. First open the buy menu with with B. Then click unlock:40. Finally choose the shotgun. In the field you will have three options hopefully this does not overload your little gnome brain.",
            //16
            "Now that you have a secondary weapon, switch to it using the scroll wheel and behold the double barrel glory.",
            //17
            "Show that bird brained fool you mean business. ….. Come on already shoot him already.",
            //18
            "While you destroyed that duck I replaced your default gun with the ricochet rifle switch to it now by using the scroll wheel.",
            //19
            "Okay with this weapon its projectiles bounce off walls. Use this to take out that cowering duck behind the garden wall. This weapon requires both skill and intelligence something you obviously lack but there is always dumb luck.",
            //20
            "Like I said dumb luck. Now switch weapons with the scroll wheel you will notice you left yourself open again and I replaced your shotgun with the rail gun. ",
            //21
            "Now that all the ducks are in a row watch them topple with the might of the unstoppable rail gun.",
            //22
            "If out in the garden you find yourself running low on the ammo for these powerful weapons you can call in the specific ammo through the buy menu. Let's go through this for the thousandth time press B then click unlock: 30 finally press use. ",
            //23
            "Hurry up and call it in already so we can just move on already. Come on just left click where you want the ammo.",
            //24
            "Since we are in a rush go pickup that mini mushroom you will notice enemies dropping them when they are killed they give you resources like the large mushrooms but you don’t have to be brought back to the shack.",
            //25
            "Go collect some ammo from that drop you just called in. This type of ammo will only work with the unlocked weapon remember this when you are out in the garden. ",
            //26
            "Now that you understand the how to unlock and use abilities found in the buy menu lets go over how to upgrade them. First click B to open the buy menu then click upgrade: 50 under drop bomb. This will make for a bigger boom when deployed.",
            //27
            "You can also upgrade yourself your puny self with this method under the passive upgrades section of the buy menu. Click upgrade 20 under max health. These passive upgrades can go to level 4 while abilities can only go to level 3.",
            //28
            "Since I am tired of saying press B to open the buy menu lets close it by either clicking X or pressing escape. Then press 3 to use the bomb ability. You can see what abilities you have access to and what abilities are off cooldown by looking at the ability bar at the bottom of your screen.",
            //New 29
            "As much as I like explosions there is nothing to drop this bomb on so let's cancel the ability by pressing right click. This will restore the cooldown. … If for whatever reason you can’t hold back your drive to see explosions and deployed the bomb you can always cancel another ability to continue.",
            //30
            "Now if you are either brave and stupid or have any care for my sanity exit this tutorial by going down the right corridor. I am required by the gnomes on high that you can also continue this tutorial by going up the north path.",
            //31
            "While you may be a heartless monster at least you have some self preservation instincts. Now throw those instincts out the window and run past that turret that is armed with live rounds.",
            //32
            "Now go make friends with that emotionless death machine. You can do this by going up to it and dragging it with the E key.",
            //33
            "WHAT how in the gnome village did that work and even ignoring that basic piece of logic, how did that turret not turn you into lawn mulch. Ahh whatever just go bring that turret over to the turret buffer in the next area. The enemy developed these buffers to make there turrets more accurate but I guess your new friend could get some use out of it.",
            //New 34
            "So not only will the turret not kill you now that you made friends with it but apparently it will shoot things for you.",
            //35
            "What if you place the turret down with E. I wonder will it go back to killing you or will it remain your one and only friend.",
            //36
            "So apparently it remains friendly, too badddd.",
            //37
            "Well that failed so let's move onto dealing with volatile explosives it should be much safer. Also that turret is starting to creep me out since I can’t imagine anything wanting to be friends with such a useless gnome.",
            //38
            "While I deeply hope an explosive barrel will be the death of you one day, for now shoot that barrel the ducks are surrounding. Out in the garden there are alot of these barrels laying around so get used to shooting them.",
            //39
            "Ahhhhh that hit the spot. Okay now go drag that barrel behind you again use E. I promise it won’t explode on contact. While it would be nice, having put so much effort into your training, at this point you dying here would probably be worse. Only probably…. don’t get your hopes up.",
            //40
            "Okay now drag that barrel into the ducks path and place it down with E. Then back away and shoot it when the duck is close.",
            //41
            "Ohh nooo this is bad, it seems the ducks are mounting an attack on my shack. Go through that teleporter it will take you right outside my shack.",
            //42
            "In the future the ducks will attack your shack but for now you will need to defend  mine. While I try to remember the door passcode be on the lookout for enemies on the minimap located at the bottom right corner of your screen. Enemies look like red E’s if close and red exclamation marks if they are far away.",
            //New 43
            "Finally got it okay go take care of that duck before he does any more damage. If you even think about letting ducks destroy my shack I will fail you. Probably for the best since a shackless gnome is a dead gnome.",
            //44
            "Ohh thank gnome my shack is still in one piece. This is bad it looks like another wave of ducks is coming. At the top of your screen you can see the countdown till they get here and when they arrive it shows how long they will stay. I have given you a massive amount of resource and full access to the buy menu as thanks and so my shack doesn't get destroyed in the next minute.",
            //45
            "Here they are for both your sake and mine I hope you manage to hold these fowl creatures off.",
            //46
            "Thanks I greatly appreciate it. However it looks like those ducks are not going to stop anytime soon. Please I am begging you stay here and keep defending my base. Don’t even think about going through that graduation portal and leaving me here alone….. Please?"

        };
        myAudioSource = GetComponent<AudioSource>();
        arrow = FindObjectOfType<AMS_TutorialPointer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerGun = player.GetComponent<AMS_GunManagement>();
        buyPlayer = player.GetComponent<AMS_BuyMenuPlayer>();
        //Start of level disable players gun
        player.GetComponent<AMS_GunManagement>().alternativeFireEnabled = false;
        player.GetComponent<AMS_GunManagement>().laser.SetActive(false);
        //Disable all of the buy menu options
        for (int i = 0; i < buyMenuOptions.Length; i++)
        {
            buyMenuOptions[i].SetActive(false);
        }
        for (int i = 0; i < tutorialTargets.Length; i++)
        {
            tutorialTargets[i].SetActive(false);
        }
        for (int i = 1; i < textUpdaters.Length; i++)
        {
            textUpdaters[i].SetActive(false);
        }
        foreach (GameObject passiveUpgrade in passiveUpgrades)
        {
            passiveUpgrade.SetActive(false);
        }
        myAudioSource.clip = audioTracks[0];
        myAudioSource.Play();

        //Disable buy menu
        player.GetComponent<AMS_BuyMenuPlayer>().enabled = false;

    }
    public void SkipToNextText()
    {
        StopAllCoroutines();
        currentDisplayText++;
        VoiceOver();
        interupttimer = 0;
        clipHasBeenPlayingFor = 0;
        //Special text changes
        if (currentDisplayText > displayText.Length)
        {
            currentDisplayText = 0;
        }
        //Enable players gun
        if (currentDisplayText == 1)
        {
            player.GetComponent<AMS_GunManagement>().gunEnabled = true;
            player.GetComponent<AMS_GunManagement>().laser.SetActive(true);
            tutorialTargets[0].SetActive(true);
            levelGate.enabled = false;
            levelGate.gameObject.GetComponent<LevelGateScript>().enabled = false;
        }
        //Go to resource area
        if (currentDisplayText == 2)
        {
            tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            player.GetComponent<AMS_GunManagement>().gunEnabled = true;
            player.GetComponent<AMS_GunManagement>().laser.SetActive(true);
            textUpdaters[1].SetActive(true);
            arrow.objectTryingToFind = textUpdaters[1];
        }
        if (currentDisplayText == 3)
        {
            tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical = Mathf.Abs(tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical);
            tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            arrow.objectTryingToFind = FindObjectOfType<BaseHealth>().gameObject;
        }
        //after collecting resources turn buy menu on
        if (currentDisplayText == 5)
        {
            arrow.objectTryingToFind = null;
            player.GetComponent<AMS_BuyMenuPlayer>().enabled = true;
            buyMenuOptions[0].SetActive(true);
        }
        //Buy Door 2nd Time
        if (currentDisplayText == 10)
        {
            buyMenuOptions[0].SetActive(false);
            FindObjectOfType<AMS_ResourceController>().currentResources = 150;
        }
        if (currentDisplayText == 11)
        {
            buyMenuOptions[2].SetActive(true);
            FindObjectOfType<AMS_ResourceController>().currentResources = 50;
            tutorialTargets[1].SetActive(true);
            arrow.objectTryingToFind = tutorialTargets[1];
        }
        //Drop bomb enemy died
        if (currentDisplayText == 13)
        {
            tutorialDoors[2].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            buyMenu.CloseMenu(true);
            buyPlayer.CancelAbility();
            player.GetComponent<AMS_BuyMenuPlayer>().enabled = false;           
            buyMenu.gameObject.SetActive(false);           
            textUpdaters[2].SetActive(true);
            arrow.objectTryingToFind = textUpdaters[2];
        }
        //Arrived in gun range
        if (currentDisplayText == 14)
        {
            tutorialDoors[2].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical = Mathf.Abs(tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical);
            tutorialDoors[2].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            tutorialTargets[2].SetActive(true);
            buyMenuOptions[2].SetActive(false);
            player.GetComponent<AMS_GunManagement>().alternativeFireEnabled = true;
        }
        if (currentDisplayText == 15)
        {
            player.GetComponent<AMS_BuyMenuPlayer>().enabled = true;
            FindObjectOfType<AMS_ResourceController>().currentResources = 40;
            buyMenuOptions[3].SetActive(true);
            buyMenuOptions[0].SetActive(false);
        }
        if (currentDisplayText == 18)
        {
            playerGun.gun1 = "Ricochet Rifle";
            playerGun.gun1maxAmmo = 40;
            playerGun.gun1currentAmmo = playerGun.gun1maxAmmo;
            playerGun.gun1currentClips = 4;
            playerGun.gun1maxFireRate = .05f;
            playerGun.gun1Bullet = playerGun.bulletTypes[3];
            buyMenu.boughtWeapon = "Ricochet Rifle";
        }
        if (currentDisplayText == 20)
        {
            playerGun.gun2 = "Rail Gun";
            playerGun.gun2maxAmmo = 10;
            playerGun.gun2currentAmmo = playerGun.gun2maxAmmo;
            playerGun.gun2currentClips = 4;
            playerGun.gun2maxFireRate = .6f;
            playerGun.gun2Bullet = playerGun.bulletTypes[2];
            buyMenu.boughtWeapon = "Rail Gun";
        }
        if (currentDisplayText == 22)
        {
            player.GetComponent<AMS_BuyMenuPlayer>().enabled = true;
            FindObjectOfType<AMS_ResourceController>().currentResources = 30;
            buyMenuOptions[3].SetActive(false);
            buyMenuOptions[4].SetActive(true);
        }
        //collect mini resource
        if (currentDisplayText == 24)
        {
            buyMenuOptions[4].SetActive(false);
            player.GetComponent<AMS_BuyMenuPlayer>().enabled = true;
            tutorialTargets[6].SetActive(true);
        }
        if (currentDisplayText == 27)
        {
            FindObjectOfType<AMS_ResourceController>().currentResources = 90;
        }
        if (currentDisplayText == 31)
        {
            tutorialDoors[4].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            //dragableTurret.SetActive(true);
            textUpdaters[6].SetActive(true);
            arrow.objectTryingToFind = textUpdaters[6];
        }
        if (currentDisplayText == 32)
        {
            tutorialDoors[5].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            arrow.objectTryingToFind = dragableTurret;
        }
        if (currentDisplayText == 33)
        {
            turretBuffer.SetActive(true);
        }
        if (currentDisplayText == 34)
        {
            tutorialDoors[7].GetComponent<AMS_TutorialDoorControl>().MakeMove();
        }
        if (currentDisplayText == 35)
        {
            player.GetComponent<KE_MainPlayer_Script>().handy = true;
        }
        if (currentDisplayText == 37)
        {
            tutorialDoors[6].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            textUpdaters[7].SetActive(true);
            arrow.objectTryingToFind = textUpdaters[7];
        }
        if (currentDisplayText == 38)
        {
            tutorialDoors[6].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical = Mathf.Abs(tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical);
            tutorialDoors[6].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            tutorialTargets[9].SetActive(true);
            tutorialTargets[10].SetActive(true);
            tutorialTargets[11].SetActive(true);
        }
        if (currentDisplayText == 39)
        {
            explosiveBarrelSpawner[0].SetActive(false);
            explosiveBarrelSpawner[1].SetActive(true);
            tutorialDoors[8].GetComponent<AMS_TutorialDoorControl>().MakeMove();
        }
        if (currentDisplayText == 41)
        {
            tutorialDoors[9].SetActive(true);
            arrow.objectTryingToFind = tutorialDoors[9];
            textUpdaters[8].SetActive(true);
            FindObjectOfType<BaseHealth>().gameObject.SetActive(false);
        }
        if (currentDisplayText == 42)
        {
            arrow.objectTryingToFind = null;
            foreach(GameObject component in practiceWaveComponets)
            {
                component.SetActive(true);
            }
        }
        if (currentDisplayText == 44)
        {
            buyPlayer.enabled = true;
            foreach (GameObject buy in buyMenuOptions)
            {
                if (buy != buyMenuOptions[3])
                {
                    buy.SetActive(true);
                }
            }
            foreach (GameObject upgrade in passiveUpgrades)
            {
                upgrade.SetActive(true);
            }
            FindObjectOfType<AMS_ResourceController>().currentResources = 500;
        }
        //Display the text
        StartCoroutine(AnimateText());
    }

    public void Update()
    {
        if (!CheckIfPlayingInterupt())
        {
            interupttimer += Time.deltaTime;
        }
        myAudioSource.pitch = Time.timeScale;
        Debug.Log(currentDisplayText);
        AfterInterupt();
        //Condition checks
        if (currentDisplayText == 0)
        {
            if (playerGun.gunEnabled)
            {
                playerGun.gunEnabled = false;
            }
        }
        //Resource Picked Up
        if (currentDisplayText == 3)
        {
            if (player.GetComponent<KE_MainPlayer_Script>().carriedObj != null)
            {
                SkipToNextText();
                tutorialDoors[12].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            }
        }
        //Resource collected
        if (currentDisplayText == 4)
        {
            if (FindObjectOfType<AMS_ResourceController>().currentResources >= 70)
            {
                tutorialDoors[12].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical = Mathf.Abs(tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical);
                tutorialDoors[12].GetComponent<AMS_TutorialDoorControl>().MakeMove();
                SkipToNextText();
            }
        }
        //Bought ammo box
        if (currentDisplayText == 5)
        {
            if (player.GetComponent<AMS_BuyMenuPlayer>().ammoBoxActive)
            {
                SkipToNextText();
            }
        }
        //Deploy ammo box
        if (currentDisplayText == 6)
        {
            if (FindObjectOfType<AMS_DeployedHealthPack>() != null)
            {
                SkipToNextText();
                tempClips = 0;
                once = false;
            }
        }
        //Reload
        if (currentDisplayText == 7)
        {
            if (once == false)
            {
                tempClips = playerGun.currentClips;
                once = true;
            }
            if (once)
            {
                if(playerGun.currentClips > tempClips)
                {
                    tempClips = playerGun.currentClips;
                }
                if (playerGun.currentClips < tempClips)
                {
                    SkipToNextText();
                }
            }
        }
        //Got ammo
        if (currentDisplayText == 8)
        {
            if (playerGun.currentClips == 5 || FindObjectOfType<AMS_Ammo_Pickup>() == null && FindObjectOfType<AMS_DeployedHealthPack>() == null)
            {
                SkipToNextText();
                levelGate.enabled = true;
                arrow.objectTryingToFind = levelGate.gameObject;
                tempClips = 0;
                once = false;
            }
        }
        //Fail to open door
        if (currentDisplayText == 9)
        {
            if (tutorialDoors[1].GetComponent<LevelGateScript>().DoorPurchaseTrigger)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SkipToNextText();
                    levelGate.gameObject.GetComponent<LevelGateScript>().enabled = true;
                    textUpdaters[4].SetActive(true);
                    arrow.objectTryingToFind = textUpdaters[4];
                }
            }
        }

        if (currentDisplayText == 11)
        {
            if (player.GetComponent<AMS_BuyMenuPlayer>().bombActive)
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 12)
        {
            if (FindObjectOfType<AMS_BuyMenuCooldownController>().currentDropBombCooldown > 3)
            {
                FindObjectOfType<AMS_BuyMenuCooldownController>().currentDropBombCooldown = 3;
            }
        }
        //Checks to see if the player has bought the secondary gun
        if (currentDisplayText == 15)
        {
            foreach (GameObject button in weaponUnlockButtonsToTurnOff)
            {
                button.SetActive(false);
            }
            if (player.GetComponent<AMS_GunManagement>().gun2 == "Shotgun")
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 16)
        {
            if (player.GetComponent<AMS_GunManagement>().currentGun == "Shotgun")
            {
                player.GetComponent<AMS_BuyMenuPlayer>().enabled = false;
                tutorialTargets[3].SetActive(true);
                SkipToNextText();
            }
        }
        if (currentDisplayText == 18)
        {
            if (player.GetComponent<AMS_GunManagement>().currentGun == "Ricochet Rifle")
            {
                tutorialTargets[4].SetActive(true);
                SkipToNextText();
            }
        }
        if (currentDisplayText == 20)
        {
            if (player.GetComponent<AMS_GunManagement>().currentGun == "Rail Gun")
            {
                tutorialTargets[5].SetActive(true);
                tutorialTargets[13].SetActive(true);
                tutorialTargets[14].SetActive(true);
                tutorialTargets[15].SetActive(true);
                SkipToNextText();
            }
        }
        if (currentDisplayText == 22)
        {
            if (player.GetComponent<AMS_BuyMenuPlayer>().ammoBoxActive && buyPlayer.ammoBoxType == "Unlock")
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 23)
        {
            if (Input.GetMouseButtonDown(0) && buyPlayer.ammoBoxActive && buyPlayer.ammoBoxType == "Unlock")
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 24)
        {
            if (FindObjectOfType<AMS_ResourceController>().currentResources == 5)
            {
                once = false;
                SkipToNextText();
            }
        }
        //Player got unlock ammo
        if (currentDisplayText == 25)
        {
            if (once == false)
            {
                if (player.GetComponent<AMS_GunManagement>().currentClips < 5 || FindObjectOfType<AMS_Ammo_Pickup>() == null && FindObjectOfType<AMS_DeployedHealthPack>() == null)
                {  
                    tempClips = playerGun.currentClips;
                    once = true;
                }
            }
            if (once)
            {
                if (player.GetComponent<AMS_GunManagement>().currentClips == 5 || FindObjectOfType<AMS_Ammo_Pickup>() == null && FindObjectOfType<AMS_DeployedHealthPack>() == null)
                {
                    FindObjectOfType<AMS_ResourceController>().currentResources = 70;
                    upgradeActive = true;
                    tempClips = 0;
                    once = false;
                    buyMenuOptions[4].SetActive(false);
                    buyMenuOptions[2].SetActive(true);
                    abilityUpgradeButtons[2].SetActive(true);
                    SkipToNextText();
                }
            }
        }
        if (currentDisplayText == 26)
        {
            if(buyMenu.levelInts[2] == 2)
            {
                SkipToNextText();
                passiveUpgrades[0].SetActive(true);
                passiveUpgrades[1].SetActive(true);
            }
        }
        if (currentDisplayText == 27)
        {
            if(buyMenu.passiveLevelInts[0] == 2)
            {
                SkipToNextText();
                buyMenuOptions[2].SetActive(false);
            }
        }
        if (currentDisplayText == 28)
        {
            if (player.GetComponent<AMS_BuyMenuPlayer>().bombActive)
            {
                upgradeActive = false;
                SkipToNextText();
            }
        }
        if (currentDisplayText == 29)
        {
            //Keep the bomb off cooldown so it can be cancelled.
            if (FindObjectOfType<AMS_BuyMenuCooldownController>().currentDropBombCooldown > 3)
            {
                FindObjectOfType<AMS_BuyMenuCooldownController>().currentDropBombCooldown = 3;
            }

            if (buyPlayer.ammoBoxActive || buyPlayer.bombActive || buyPlayer.healthPackActive || buyPlayer.minesActive || buyPlayer.weaknessZoneActive || buyPlayer.spikeTrapActive || buyPlayer.tauntTotemActive)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    buyPlayer.CancelAbility();
                    buyPlayer.enabled = false;
                    tutorialDoors[3].GetComponent<AMS_TutorialDoorControl>().MakeMove();
                    textUpdaters[3].SetActive(true);
                    textUpdaters[5].SetActive(true);
                    arrow.objectTryingToFind = textUpdaters[5];
                    SkipToNextText();
                }
            }
        }
        if (currentDisplayText == 32)
        {
            if (player.GetComponent<KE_MainPlayer_Script>().carriedObj == dragableTurret)
            {
                tutorialDoors[3].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical = Mathf.Abs(tutorialDoors[0].GetComponent<AMS_TutorialDoorControl>().moveAmountVertical);
                tutorialDoors[3].GetComponent<AMS_TutorialDoorControl>().MakeMove();
                SkipToNextText();
                textUpdaters[9].SetActive(true);
                arrow.objectTryingToFind = textUpdaters[9];
            }
        }
        if (currentDisplayText == 33)
        {
            player.GetComponent<KE_MainPlayer_Script>().handy = false;
        }
        if (currentDisplayText == 34)
        {
            player.GetComponent<KE_MainPlayer_Script>().handy = false;
            if (myAudioSource.clip == audioTracks[34])
            {
                if (!myAudioSource.isPlaying)
                {
                    if (!tutorialTargets[7].activeSelf)
                    {
                        tutorialTargets[7].SetActive(true);
                    }
                }
            }
        }
        if (currentDisplayText == 35)
        {
            if (player.GetComponent<KE_MainPlayer_Script>().carriedObj == null)
            {
                SkipToNextText();
                Destroy(dragableTurret.GetComponent<DragScript>());
            }
        }
        if (currentDisplayText == 36)
        {
            if (myAudioSource.clip == audioTracks[36])
            {
                clipHasBeenPlayingFor += Time.deltaTime;
                if (clipHasBeenPlayingFor > myAudioSource.clip.length / 1.1)
                {
                    if (!tutorialTargets[8].activeSelf)
                    {
                        tutorialTargets[8].SetActive(true);
                    }
                    timeSpent += Time.deltaTime;
                }
            }
            if (timeSpent >= 10)
            {
                tutorialTargets[8].SetActive(false);
                SkipToNextText();
            }
        }

        if (currentDisplayText == 39)
        {
            //Debug.Log("you will have to find a new way of doing this as dragging has been changed");
            if (player.GetComponent<KE_MainPlayer_Script>().carriedObj != null)
            {
                tutorialTargets[12].SetActive(true);
                SkipToNextText();
            }
        }
        //Practice wave
        if (currentDisplayText == 42)
        {
            if (myAudioSource.clip == audioTracks[42])
            {
                clipHasBeenPlayingFor += Time.deltaTime;
                if (clipHasBeenPlayingFor > myAudioSource.clip.length / 1.5)
                {
                    if (practiceWaveComponets[0].GetComponentInChildren<WaveSpawner>().enabled != true)
                    {
                        practiceWaveComponets[0].GetComponentInChildren<WaveSpawner>().enabled = true;
                    }
                }
            }
            if(practiceWaveComponets[0].GetComponentInChildren<WaveSpawner>().enabled == true)
            {
                if(GameObject.FindGameObjectWithTag("Default_Enemy"))
                {
                    once2 = true;
                }
                else if(once2)
                {
                    SkipToNextText();
                }

            }
            if (FindObjectOfType<BaseHealth>().currentHP < FindObjectOfType<BaseHealth>().maxHP)
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 43)
        {
            if (GameObject.FindGameObjectWithTag("Default_Enemy"))
            {
                arrow.objectTryingToFind = GameObject.FindGameObjectWithTag("Default_Enemy");
            }
            tutorialDoors[10].GetComponent<AMS_TutorialDoorControl>().MakeMove();
            if (FindObjectOfType<WaveSpawner>().enemiesSpawned.Count == 0)
            {
                arrow.objectTryingToFind = null;
                upgradeActive = true;
                abilityUpgradeButtons[0].SetActive(true);
                abilityUpgradeButtons[2].SetActive(true);
                abilityUpgradeButtons[3].SetActive(true);
                SkipToNextText();
            }
        }
        if (currentDisplayText == 44)
        {
            if (FindObjectOfType<WaveSpawner>().currentWaveNumber == 2)
            {
                SkipToNextText();
            }
        }
        if (currentDisplayText == 45)
        {
            if (FindObjectOfType<WaveSpawner>().enemiesSpawned.Count != 0)
            {
                once = true;
            }
            if (once)
            {
                if (FindObjectOfType<WaveSpawner>().enemiesSpawned.Count == 0)
                {
                    tutorialDoors[11].SetActive(true);
                    arrow.objectTryingToFind = tutorialDoors[11];
                    SkipToNextText();
                }
            }
        }
        //Dont run out of ammo
        if (playerGun.currentClips == 0)
        {
            playerGun.currentClips += 1;
        }
        if (playerGun.defaultAlternateFireAmmo == 0)
        {
            playerGun.defaultAlternateFireAmmo += 1;
        }
        //Player cant die
        if (player.GetComponent<AMS_Health_Management>().currentHealth < 60)
        {
            player.GetComponent<AMS_Health_Management>().currentHealth = 60;
        }
        //Base cant die
        if (FindObjectOfType<BaseHealth>() != null)
        {
            if (FindObjectOfType<BaseHealth>().currentHP < 40)
            {
                FindObjectOfType<BaseHealth>().currentHP = 40;
            }
        }
        foreach (GameObject upgradeButton in abilityUpgradeButtons)
        {
            if (upgradeButton.activeSelf)
            {
                if (!upgradeActive)
                {
                    upgradeButton.SetActive(false);
                }
            }
        }
    }


    IEnumerator AnimateText()
    {
        for (int i = 0; i < (displayText[currentDisplayText].Length + 1); i++)
        {
            textBox.text = displayText[currentDisplayText].Substring(0, i);
            yield return new WaitForSeconds(.005f);
        }
    }
    private void VoiceOver()
    {
        if (myAudioSource.isPlaying)
        {
            //Interupt
            if (!CheckIfPlayingInterupt())
            {
                if (interupttimer > myAudioSource.clip.length * 0.8f)
                {
                    myAudioSource.Stop();
                    myAudioSource.clip = audioTracks[currentDisplayText];
                    myAudioSource.Play();
                }
                else
                {
                    myAudioSource.Stop();
                    myAudioSource.clip = interuptNoise[InteruptNoiseRandom()];
                    myAudioSource.Play();
                }

            }   
        }
        else if (audioTracks[currentDisplayText] != null)
        {
            myAudioSource.clip = audioTracks[currentDisplayText];
            myAudioSource.Play();
        }
    }

    private int InteruptNoiseRandom()
    {
        tempNumber = Random.Range(0, 3);
        return tempNumber;
    }

    private void AfterInterupt()
    {
        if (!myAudioSource.isPlaying)
        {
            if (myAudioSource.clip == interuptNoise[tempNumber])
            {
                myAudioSource.clip = audioTracks[currentDisplayText];
                myAudioSource.Play();
            }
        }
    }

    private bool CheckIfPlayingInterupt()
    {
        for (int i = 0; i < interuptNoise.Length; i++)
        {
            if (myAudioSource.clip == interuptNoise[i])
            {
                return true;
            }
        }
        return false;
    }
}

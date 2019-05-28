using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_BuyMenuCooldownController : MonoBehaviour
{

    //Cooldown variables
    public bool[] purchasableAbilities;

    //Drop Bomb
    public float dropBombCooldown;
    [HideInInspector]
    public float currentDropBombCooldown = 0;
    public GameObject dropBombButton;
    public Image dropBombCooldownCircle;
    //Ammo Box
    public float ammoBoxCooldown;
    [HideInInspector]
    public float currentammoBoxCooldown = 0;
    public GameObject ammoBoxButton;
    public Image ammoBoxCooldownCircle;
    //Health Pack
    public float healthPackCooldown;
    [HideInInspector]
    public float currenthealthPackCooldown = 0;
    public GameObject healthPackButton;
    public Image healthPackCooldownCircle;
    //Unlock Ammo
    public float unlockAmmoCooldown;
    [HideInInspector]
    public float currentUnlockAmmoCooldown = 0;
    public GameObject unlockAmmoButton;
    public GameObject weaponUnlock;
    public Image unlockAmmoCooldownCircle;
    //Mines
    public float minesCooldown;
    [HideInInspector]
    public float currentminesCooldown = 0;
    public GameObject minesControlButton;
    public Image minesControlCooldownCircle;
    //Base Health Restore
    public float baseRestoreCooldown;
    [HideInInspector]
    public float currentBaseRestoreCooldown = 0;
    public GameObject baseRestoreButton;
    public Image baseRestoreCooldownCircle;
    //Weakness zone
    public float weaknessZoneCooldown;
    [HideInInspector]
    public float currentWeaknessZoneCooldown = 0;
    public GameObject weaknessZoneButton;
    public Image weaknessZoneCooldownCircle;
    //Spike Trap
    public float spikeTrapCooldown;
    [HideInInspector]
    public float currentSpikeTrapCooldown = 0;
    public GameObject spikeTrapButton;
    public Image spikeTrapCooldownCircle;
    //Taunt Totem
    public float tauntTotemCooldown;
    [HideInInspector]
    public float currentTauntTotemCooldown = 0;
    public GameObject tauntToemButton;
    public Image tauntTotemCooldownCircle;
    //Not Enough
    public GameObject notEnoughMessage;
    public float disapperMessageTimer;
    private float timePassed;
    private bool once = true;
    public AudioClip notEnoughSound;
    public AMS_BuyControls buyMenu;
    public float notEnoughSoundTimer = 1f;

    //Cooldown Finished Sound
    public AudioClip cooldownOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Drop Bomb
        if (currentDropBombCooldown > 0)
        {
            currentDropBombCooldown -= Time.deltaTime;
            dropBombCooldownCircle.enabled = true;
            dropBombCooldownCircle.fillAmount = currentDropBombCooldown / dropBombCooldown;
        }
        else
        {
            if (!dropBombButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[2])
                {
                    dropBombButton.SetActive(true);
                    dropBombCooldownCircle.enabled = false;
                    purchasableAbilities[2] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Ammo Box
        if (currentammoBoxCooldown > 0)
        {
            currentammoBoxCooldown -= Time.deltaTime;
            ammoBoxCooldownCircle.enabled = true;
            ammoBoxCooldownCircle.fillAmount = currentammoBoxCooldown / ammoBoxCooldown;
        }
        else
        {
            if (!ammoBoxButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[0])
                {
                    ammoBoxButton.SetActive(true);
                    ammoBoxCooldownCircle.enabled = false;
                    purchasableAbilities[0] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //HealthPack
        if (currenthealthPackCooldown > 0)
        {
            currenthealthPackCooldown -= Time.deltaTime;
            healthPackCooldownCircle.enabled = true;
            healthPackCooldownCircle.fillAmount = currenthealthPackCooldown / healthPackCooldown;
        }
        else
        {
            if (!healthPackButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[1])
                {
                    healthPackButton.SetActive(true);
                    healthPackCooldownCircle.enabled = false;
                    purchasableAbilities[1] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Unlock Ammo
        if (currentUnlockAmmoCooldown > 0)
        {
            currentUnlockAmmoCooldown -= Time.deltaTime;
            unlockAmmoCooldownCircle.enabled = true;
            unlockAmmoCooldownCircle.fillAmount = currentUnlockAmmoCooldown / unlockAmmoCooldown;
        }
        else
        {
            if (!unlockAmmoButton.activeSelf && !weaponUnlock.activeSelf)
            {
                if (buyMenu.unlockedAbilities[3])
                {
                    unlockAmmoButton.SetActive(true);
                    unlockAmmoCooldownCircle.enabled = false;
                    purchasableAbilities[3] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Mines
        if (currentminesCooldown > 0)
        {
            currentminesCooldown -= Time.deltaTime;
            minesControlCooldownCircle.enabled = true;
            minesControlCooldownCircle.fillAmount = currentminesCooldown / minesCooldown;
        }
        else
        {
            if (!minesControlButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[4])
                {
                    minesControlButton.SetActive(true);
                    minesControlCooldownCircle.enabled = false;
                    purchasableAbilities[4] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Base Health Restore
        if (currentBaseRestoreCooldown > 0)
        {
            currentBaseRestoreCooldown -= Time.deltaTime;
            baseRestoreCooldownCircle.enabled = true;
            baseRestoreCooldownCircle.fillAmount = currentBaseRestoreCooldown / baseRestoreCooldown;
        }
        else
        {
            if(!baseRestoreButton.activeSelf)
            {
                if(buyMenu.unlockedAbilities[5])
                {
                    baseRestoreButton.SetActive(true);
                    baseRestoreCooldownCircle.enabled = false;
                    purchasableAbilities[5] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Weakness Zone
        if (currentWeaknessZoneCooldown > 0)
        {
            currentWeaknessZoneCooldown -= Time.deltaTime;
            weaknessZoneCooldownCircle.enabled = true;
            weaknessZoneCooldownCircle.fillAmount = currentWeaknessZoneCooldown / weaknessZoneCooldown;
        }
        else
        {
            if (!weaknessZoneButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[6])
                {
                    weaknessZoneButton.SetActive(true);
                    weaknessZoneCooldownCircle.enabled = false;
                    purchasableAbilities[6] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //SpikeTrap
        if (currentSpikeTrapCooldown > 0)
        {
            currentSpikeTrapCooldown -= Time.deltaTime;
            spikeTrapCooldownCircle.enabled = true;
            spikeTrapCooldownCircle.fillAmount = currentSpikeTrapCooldown / spikeTrapCooldown;
        }
        else
        {
            if (!spikeTrapButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[7])
                {
                    spikeTrapButton.SetActive(true);
                    spikeTrapCooldownCircle.enabled = false;
                    purchasableAbilities[7] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //Taunt Totem
        if (currentTauntTotemCooldown > 0)
        {
            currentTauntTotemCooldown -= Time.deltaTime;
            tauntTotemCooldownCircle.enabled = true;
            tauntTotemCooldownCircle.fillAmount = currentTauntTotemCooldown / tauntTotemCooldown;
        }
        else
        {
            if (!tauntToemButton.activeSelf)
            {
                if (buyMenu.unlockedAbilities[8])
                {
                    tauntToemButton.SetActive(true);
                    tauntTotemCooldownCircle.enabled = false;
                    purchasableAbilities[8] = true;
                    GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(cooldownOver);
                }
            }
        }
        //NotEnoughMessage
        if (notEnoughMessage.activeSelf)
        {
            if (once)
            {
                GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(notEnoughSound);
                once = false;
            }
            if (disapperMessageTimer < timePassed)
            {
                notEnoughMessage.SetActive(false);
                timePassed = 0;
                once = true;
            }
            if(notEnoughSoundTimer <= 0)
            {
                GameObject.FindObjectOfType<AudioSwitcherScript>().KillSound(notEnoughSound);
            }
            else
            {
                timePassed += Time.deltaTime;
            }
        }
    }
}

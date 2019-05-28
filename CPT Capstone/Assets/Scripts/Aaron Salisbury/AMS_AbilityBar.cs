using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_AbilityBar : MonoBehaviour
{
    //Order ammo, health, bomb, unlock ammo, mines
    public GameObject[] greyBoxs;
    public Image[] cooldownCircles;
    public Text HoverAbilityBarText;


    public AMS_BuyMenuCooldownController cooldownController;
    private float[] cooldownValues;
    // Start is called before the first frame update
    void Start()
    {
        HoverAbilityBarText = GameObject.Find("HoverAbilityBarText").GetComponent<Text>();
        HoverAbilityBarText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownValues = new float[]{
            cooldownController.currentammoBoxCooldown / cooldownController.ammoBoxCooldown,
            cooldownController.currenthealthPackCooldown / cooldownController.healthPackCooldown,
            cooldownController.currentDropBombCooldown / cooldownController.dropBombCooldown,
            cooldownController.currentUnlockAmmoCooldown / cooldownController.unlockAmmoCooldown,
            cooldownController.currentminesCooldown / cooldownController.minesCooldown,
            cooldownController.currentBaseRestoreCooldown / cooldownController.baseRestoreCooldown,
            cooldownController.currentWeaknessZoneCooldown / cooldownController.weaknessZoneCooldown,
            cooldownController.currentSpikeTrapCooldown / cooldownController.spikeTrapCooldown,
            cooldownController.currentTauntTotemCooldown / cooldownController.tauntTotemCooldown};
        for (int i = 0; i <= greyBoxs.Length - 1; i++)
        {
            if (cooldownController.purchasableAbilities[i])
            {
                greyBoxs[i].SetActive(false);
                cooldownCircles[i].enabled = false;
            }
            else
            {
                greyBoxs[i].SetActive(true);
                cooldownCircles[i].enabled = true;
                cooldownCircles[i].fillAmount = cooldownValues[i];
            }
            
            

        }
    }

    public void mouseOverAbilityBar()
    {
        HoverAbilityBarText.enabled = true;
        HoverAbilityBarText.GetComponent<Text>().text = "Press [B] to open the Buy Menu";
    }

    public void mouseExitAbilityBar()
    {
        HoverAbilityBarText.enabled = false;
    }
}

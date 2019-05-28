using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlternateReloadSlider : MonoBehaviour
{
    public AMS_GunManagement gunScript;
    public Slider AltReloadSlider;
    public float MaxCoolDown;
    public float CoolDownRemaining;
    public GameObject AlternateFireFill;

    private void Start()
    {
        gunScript = FindObjectOfType<AMS_GunManagement>();
        MaxCoolDown = gunScript.defaultAlternativeMaxFireRate;
        AlternateFireFill = GameObject.Find("AlternateFireFill");
        AlternateFireFill.SetActive(false);
        
        //CoolDownRemaining = MaxCoolDown;
    }

    private void Update()
    {
            if (gunScript.defaultAlternativeCurrentFireRate < MaxCoolDown)
        {
            AltReloadSlider.value = CaculateCoolDown();
        }  
            else
        {
            AlternateFireFill.SetActive(false);
        }
        
    }


    float CaculateCoolDown()
    {
       AlternateFireFill.SetActive(true);
        CoolDownRemaining = gunScript.defaultAlternativeCurrentFireRate;
        var tempCoolDown = 1 - CoolDownRemaining / MaxCoolDown;
        tempCoolDown = Mathf.Max(tempCoolDown, 0.07f);
        return (tempCoolDown);

    }

}

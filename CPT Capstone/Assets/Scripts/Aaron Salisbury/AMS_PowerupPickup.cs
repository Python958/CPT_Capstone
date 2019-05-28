using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_PowerupPickup : MonoBehaviour
{
    //public AMS_GunManagement gunManagement;
    public string powerupName;
    public float powerupLength;
    public int scoreGainedOnPickup = 0;
    public float despawnLength = 10;
    public GuiTimerScript powerupNotification;
    public AudioClip powerupSound;

    // Start is called before the first frame update
    void Start()
    {
    //    gunManagement = GameObject.Find("MainPlayer").GetComponent<AMS_GunManagement>();
        var timer = gameObject.AddComponent<DespawnTimer>();
        timer.timerStart = despawnLength;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PowerupPFI();
            AMS_ScoreController.increaseScore(scoreGainedOnPickup);
            col.GetComponent<AMS_GunManagement>().powerup = powerupName;
            col.GetComponent<AMS_GunManagement>().powerupLength = powerupLength;
            col.GetComponent<AMS_GunManagement>().powerupMax = powerupLength;
            GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(powerupSound);
            Destroy(gameObject);
           
        }
    }

    void PowerupPFI()
    {
        powerupNotification = FindObjectOfType<GuiTimerScript>();
        if(powerupNotification != null)
        {
            powerupNotification.SetUpPowerupMessage(powerupName);
        }
        else { Debug.Log("can't find the powerup GUI message script"); }
    }

}

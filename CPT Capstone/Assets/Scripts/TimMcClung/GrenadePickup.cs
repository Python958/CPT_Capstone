using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadePickup : MonoBehaviour
{
    private AMS_GunManagement gun;
    public string type;
    
    //public AMS_GunManagement gunManagement;
    public int scoreGainedOnPickup = 0;
    public float despawnLength = 10;
    public AudioClip powerupSound;

    // Start is called before the first frame update
    void Start()
    {
        //    gunManagement = GameObject.Find("MainPlayer").GetComponent<AMS_GunManagement>();
        var timer = gameObject.AddComponent<DespawnTimer>();
        timer.timerStart = despawnLength;

        gun = FindObjectOfType<AMS_GunManagement>();
        if(gun == null) { Debug.Log("no gun found for player"); }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(gun != null) { gun.AddAmmo(type); }
            AMS_ScoreController.increaseScore(scoreGainedOnPickup);
            if(powerupSound != null) { GameObject.FindObjectOfType<AudioSwitcherScript>().PlaySound(powerupSound); }
            Destroy(gameObject);

        }
    }
}

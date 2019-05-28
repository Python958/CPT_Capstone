using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiTimerScript : MonoBehaviour
{
    public Text powerupNotification;
    public float maxGUICountdown;
    private float currentGUICountdown = 0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (powerupNotification.gameObject.activeSelf)
        {
            currentGUICountdown -= Time.deltaTime;
            if(currentGUICountdown <= 0f) { powerupNotification.gameObject.SetActive(false); }
        }
    }

    public void SetUpPowerupMessage(string powerupName)
    {
        powerupNotification.gameObject.SetActive(true);
        currentGUICountdown = maxGUICountdown;
        powerupNotification.text = powerupName + " Activated!";
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveGUI : MonoBehaviour
{
    public ConsoleDebugScript consoleDebugScript;
    public WaveSpawner waveSpawner;
    public Text TimerText; //CurrentWaveTime
    public Text CurrentWaveNumber;
    public Text CooldownTimerText;
    public int ActiveWaveNumber;
    public float startTimer;
    public GameObject StartWaveButton;
    private bool CoolDownStarted;


    // Start is called before the first frame update
    void Start()
    {
        if(consoleDebugScript == null) { consoleDebugScript = GameObject.FindObjectOfType<ConsoleDebugScript>(); }
        if (waveSpawner == null) { waveSpawner = GameObject.FindObjectOfType<WaveSpawner>(); }
        if(waveSpawner == null) { gameObject.SetActive(false); Debug.Log("couldn't find a wave spawner for GUI"); }
        startTimer = Time.deltaTime; //Design decision should this be counting down from "x" or counting up;
    }

    // Update is called once per frame
    void Update()
    {
        ActiveWaveNumber = waveSpawner.currentWaveNumber;
        TimerFunction();
        WaveNumber();
        ActivateButton();
    }


    public void TimerFunction()
    {
        float t = waveSpawner.currentWaveTime;//current raw time including possibly negative numbers
        CoolDownStarted = false;
        if (t == 0f) { TimerText.text = "Kill The Remaining "+ waveSpawner.enemiesSpawned.Count + " Enemies"; } //if timer is stopped at zero we are waiting for remaining enemy to be killed
        else
        {
            string title = "Wave Time: ";

            if (t < 0f)
            {
                t = waveSpawner.coolDownTimer + t;
                title = "Cool Down Time: ";
                CoolDownStarted = true;

            }//if we are in negative it is because we are in cool down

            int minutesInt = (int)t / 60;
            minutesInt = Mathf.Max(0, minutesInt);
            string minutes = minutesInt.ToString();

            int secondsInt = (int)t % 60;
            secondsInt = Mathf.Max(0, secondsInt);
            string seconds = secondsInt.ToString();

            TimerText.text = title + minutes + ":" + seconds;
        }
    }

    public void WaveNumber()
    {
        CurrentWaveNumber.text = "Wave " + ActiveWaveNumber;
    }

    public void SkipCoolDown()
    {
        consoleDebugScript.ZeroWaveTime();
    }

    public void ActivateButton()
    {
        if (CoolDownStarted == false)
        {
            StartWaveButton.SetActive(false); 
        }
        else
        {
            StartWaveButton.SetActive(true);
        }
    }
}

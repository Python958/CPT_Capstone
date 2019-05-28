using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOutcome : MonoBehaviour
{

    public AMS_Health_Management healthManagementScript;
    public BaseHealth baseHealthScript;
    public WaveSpawner waveSpawnerScript;
    public bool PlayerDied = false;
    public bool BaseWasDestroyed = false;

    public GameObject LevelOutcomeObject;
    public Text OutcomeText;

    public float CountdownLoadTimer = 2f;
    private float timeElapsed;

    public bool SceneLoaded = false;
    public bool DidThePlayerWin;
    public bool DidThePlayerLose;
    private bool coroutineStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        DidThePlayerWin = false;
        DidThePlayerLose = false;
    }

    // Update is called once per frame
    void Update()
    {
        LoseCondition();
        WinCondition();
    }

    private void LoseCondition()
    {
        
        if (baseHealthScript == null)
        {
            if(healthManagementScript.Arena != true)
            {
                DidThePlayerLose = false;
                baseHealthScript = GameObject.Find("BaseHealth").GetComponent<BaseHealth>();
            }
            else
            {
                baseHealthScript = null;
            }
        }

        if (baseHealthScript != null && baseHealthScript.currentHP <= 0)
        {
            BaseWasDestroyed = true;
            DidThePlayerLose = true;
            StartCoroutine(SceneLoad());
            //Debug.Log(timeElapsed);
            OutcomeText.GetComponent<Text>().text = "GAME OVER! YOUR BASE WAS DESTROYED";
            LevelOutcomeObject.SetActive(true);
        }
        else if (healthManagementScript.currentHealth <= 0)
        {
            PlayerDied = true;
            if (healthManagementScript.Arena == true)
            {
                DidThePlayerLose = false;
                DidThePlayerWin = true;
                OutcomeText.GetComponent<Text>().text = "GREAT FIGHT! YOU HELD YOUR GROUND FOR AS LONG AS YOU COULD!";
                LevelOutcomeObject.SetActive(true);
                StartCoroutine(SceneLoad());

            }
            else
            {
                DidThePlayerLose = true;
                StartCoroutine(SceneLoad());
                //Debug.Log(timeElapsed);
                OutcomeText.GetComponent<Text>().text = "GAME OVER! YOU WERE KILLED";
                LevelOutcomeObject.SetActive(true);
            }
        }
        else
        {
            PlayerDied = false;
            DidThePlayerLose = false;
            // Debug.Log("The Player has not lost");
            //Debug.Log(timeElapsed);
            BaseWasDestroyed = false;
            LevelOutcomeObject.SetActive(false);
        }
    }

    private void WinCondition()
    {
        if (!coroutineStarted)
        {
            if (waveSpawnerScript.WinningWaveWasBeaten)
            {
                DidThePlayerWin = true;
                OutcomeText.GetComponent<Text>().text = "YOU WIN! GNOMES RULES!";
                LevelOutcomeObject.SetActive(true);
                StartCoroutine(SceneLoad());
                coroutineStarted = true;
                Debug.Log("Coroutine Started");
            }
            else if (healthManagementScript.bossDead == true)
            {
                DidThePlayerWin = true;
                OutcomeText.GetComponent<Text>().text = "YOU DEFATED THE BOSS! YOU HAVE WON!";
                LevelOutcomeObject.SetActive(true);
                StartCoroutine(SceneLoad());
                coroutineStarted = true;
            }
        }
    }

    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(5f);

        if (DidThePlayerWin == true)
        {
            AMS_UniversalFunctions.GoToResultsScreen(true);
        }
    
        else if (DidThePlayerLose == true)
        {
            AMS_UniversalFunctions.GoToResultsScreen(false);
        }
    }
}

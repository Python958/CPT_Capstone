using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMS_GUIUpdater : MonoBehaviour
{
    public Text scoreText;
    public Image baseHealthbar2;
    public Slider baseHealthbar;
    private BaseHealth currentbase;
    //Score Ranks are as Follows SS,S,A,B,C,D,F
    [Tooltip("Score Ranks are as Follows SS,S,A,B,C,D,F   F does not need to be updated it is always zero")]
    public int[] scoreRanks;
    [Tooltip("First number is the par time second number is how much it can be off and still get a bonus. Third number is the bonus. This time is in seconds")]
    public float[] parTimes;
    public Text displayTimeText;
    private float timeOnStart;
    private float currentTime;

    //base health alert
    private float baseAlertOffTime = .1f;
    private float baseAlertOnTime = .5f;
    private float baseAlertCurrentTime;
    private float baseAlertPercent = .25f;

    //flashing alert
    private AMS_Health_Management playerHealth;
    public GameObject healthAlert;
    private float alertTime = .25f;
    private float alertPercent = .25f;
    private float alertCurrentTime;

    public Text doubleAbilityText;
    private float doubleTextTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentbase = FindObjectOfType<BaseHealth>();
        AMS_ScoreController.rankScores = scoreRanks;
        AMS_ScoreController.score = 0;
        AMS_ScoreController.scoreMultiplier = 1;
        AMS_ScoreController.parTimes = parTimes;
        timeOnStart = Time.time;

        baseAlertCurrentTime = 0f;
        alertCurrentTime = 0f;
        var playerScript = FindObjectOfType<KE_MainPlayer_Script>();
        if(playerScript != null)
        {
            var playerObj = playerScript.gameObject;
            playerHealth = playerObj.GetComponent<AMS_Health_Management>();
            if(playerHealth == null) { Debug.Log("player health was not where expected"); }

        }
        else { Debug.Log("does not seem to be a player in the level"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentbase != null) { baseHealthbar.value = currentbase.currentHP / currentbase.maxHP; }
        scoreText.text = AMS_ScoreController.scoreMultiplier + "X" + " Score: " + AMS_ScoreController.score;
        //Time
        currentTime = Time.time - timeOnStart;
        AMS_ScoreController.timeTaken = currentTime;
        string currentDisplayTime = ((int)currentTime / 60).ToString() + ":" + (currentTime % 60).ToString("f0");
        string displayParTime = ((int)parTimes[0] / 60).ToString() + ":" + (parTimes[0] % 60).ToString("f0");
        displayTimeText.text = "Current Time "+ currentDisplayTime + " || " + "Par Time " + displayParTime;

        OperatePlayerHealthAlert();
        OperateBaseHealthAlert();
        UpdateDoubleAbilityText();
    }

    private void OperatePlayerHealthAlert()
    {
        var healthPercent = playerHealth.currentHealth / playerHealth.maxHealth;
        if(healthPercent <= alertPercent)
        {
            alertCurrentTime += Time.deltaTime;
            if(alertCurrentTime >= alertTime)
            {
                alertCurrentTime = 0f;
                healthAlert.SetActive(!healthAlert.activeSelf);
            }
        }
        else if(healthAlert.activeSelf == true)
        {
            healthAlert.SetActive(false);
        }
    }

    private void OperateBaseHealthAlert()
    {
        currentbase = FindObjectOfType<BaseHealth>();
        if (currentbase != null)
        {
            var healthPercent = currentbase.currentHP / currentbase.maxHP;
            if (healthPercent <= baseAlertPercent)
            {
                baseAlertCurrentTime += Time.deltaTime;
                var targetTime = baseHealthbar.gameObject.activeSelf ? baseAlertOnTime : baseAlertOffTime;

                if (baseAlertCurrentTime >= targetTime)
                {
                    baseAlertCurrentTime = 0f;
                    baseHealthbar.gameObject.SetActive(!baseHealthbar.gameObject.activeSelf);
                }
            }
            else if (baseHealthbar.gameObject.activeSelf == false)
            {
                baseHealthbar.gameObject.SetActive(true);
            }
        }
    }
    private void UpdateDoubleAbilityText()
    {
        if (doubleAbilityText.enabled)
        {
            doubleTextTime = doubleTextTime - Time.deltaTime;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                doubleAbilityText.enabled = false;
                doubleTextTime = 5;
            }
            if (doubleTextTime <= 0)
            {
                doubleAbilityText.enabled = false;
                doubleTextTime = 10;
            }
        }
    }
}
